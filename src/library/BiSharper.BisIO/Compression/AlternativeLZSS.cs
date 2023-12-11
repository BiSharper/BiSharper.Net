using System.Text;

namespace BiSharper.BisIO.Compression
{
    public sealed class AlternativeLZSS
    {
        //break all the shit
        private const int N = 4096, F = 18, Threshold = 2;
        private const byte Fill = 0x20;

        /// <summary>
        ///     Index for root of binary search trees
        /// </summary>
        private const int Nil = N;
        /// <summary>
        ///     These constitute binary search trees.
        /// </summary>
        private static readonly int[] PreviousChildren = new int[N + 1], NextChildren = new int[N + 257], Parents = new int[N + 1];
        /// <summary>
        ///     The ring buffer of size N with extra F-1 bytes to facilitate string comparison.
        /// </summary>
        private static readonly byte[] TextBuffer = new byte[N + F - 1];
        /// <summary>
        ///     The length and position of the longest match found.
        ///     These are set by the InsertNode() procedure.
        /// </summary>
        private static int _matchPosition, _matchLength;

        public static int Decode(byte[] input, out byte[] output, uint length)
        {
            var textBuffer = new byte[N + F - 1];
            output = new byte[length];
            if (length <= 0) 
                return 0;                       //Failed here no data
            
            int i, flags = 0, r = N - F, iSrc = 0;
            uint iDst = 0, bytesLeft = length;
            
            Array.Fill(textBuffer, Fill);

            while (bytesLeft > 0 && !CheckBounds())
            {
                int c;
                if (((flags >>= 1) & 256) == 0)
                {
                    if (CheckBounds())
                        return output.Length;   //Failed here out of bounds
                    
                    c = input[iSrc++];
                    flags = c | 0xff00;
                }

                if ((flags & 1) != 0)
                {
                    if (CheckBounds())
                        return output.Length;   //Failed here out of bounds
                    
                    c = input[iSrc++];

                    // save byte
                    output[iDst++] = (byte)c;
                    bytesLeft--;
                    // continue decompression
                    TextBuffer[r++] = (byte)c;
                    r &= N - 1;
                }
                else
                {
                    if (CheckBounds())
                        return output.Length;   //Failed here out of bounds
                    
                    i = input[iSrc++];
                    if (CheckBounds())
                        return output.Length;   //Failed here out of bounds
                    
                    int j = input[iSrc++];

                    i |= (j & 0xf0) << 4;
                    j &= 0x0f;
                    j += Threshold;

                    int ii = r - i,
                        jj = j + ii;

                    if (j + 1 > bytesLeft)
                    {
                        return output.Length;
                    }

                    for (; ii <= jj; ii++)
                    {
                        c = TextBuffer[ii & (N - 1)];

                        // save byte
                        output[iDst++] = (byte)c;
                        bytesLeft--;
                        // continue decompression
                        TextBuffer[r++] = (byte)c;
                        r &= N - 1;
                    }
                }
            }

            return output.Length;

            bool CheckBounds() => input.Length <= iSrc;
        }

        public static uint Encode(Stream inputStream, out uint outputSize)
        {
            if (!inputStream.CanRead)
            {
                throw new IOException("Cannot read from the provided stream.");
            }

            using var memoryStream = new MemoryStream();
            inputStream.CopyTo(memoryStream);

            var encodedStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(encodedStream, Encoding.UTF8, true);

            outputSize = Encode(memoryStream.ToArray(), binaryWriter);

            encodedStream.Seek(0, SeekOrigin.Begin);
            return outputSize;
        }

        public static uint Encode(Stream inputStream, BinaryWriter output)
        {
            if (!inputStream.CanRead)
            {
                throw new IOException("Cannot read from the provided stream.");
            }

            using var memoryStream = new MemoryStream();
            inputStream.CopyTo(memoryStream);
            return Encode(memoryStream.ToArray(), output);
        }

        public static uint Encode(byte[] input, BinaryWriter output)
        {
            int i, len, r = N - F, s = 0, stopPos = input.Length, inputIdx = 0;
            
            byte mask, codeBufIdx = mask = 1;
            uint codeSize = 0;

            InitTree();

            var codeBuf = new byte[17];

            codeBuf[0] = 0;

            for (i = s; i < r; i++)
            {
                TextBuffer[i] = Fill;
            }

            for (len = 0; len < F && inputIdx < stopPos; len++)
            {
                TextBuffer[r + len] = input[inputIdx++];
            }

            if (len == 0)
            {
                return 0; /* text of size zero */
            }

            for (i = 1; i <= F; i++)
            {
                InsertNode(r - i);
            }

            InsertNode(r);

            do
            {
                if (_matchLength > len)
                {
                    _matchLength = len;
                }

                if (_matchLength <= Threshold)
                {
                    _matchLength = 1;
                    codeBuf[0] |= mask;
                    codeBuf[codeBufIdx++] = TextBuffer[r];
                }
                else
                {
                    var encodedPosition = (r - _matchPosition) & (N - 1);
                    codeBuf[codeBufIdx++] = (byte)encodedPosition;
                    codeBuf[codeBufIdx++] = (byte)(((encodedPosition >> 4) & 0xf0) | (_matchLength - (Threshold + 1)));
                }

                if ((mask <<= 1) == 0)
                {
                    output.Write(codeBuf, 0, codeBufIdx);
                    codeSize += codeBufIdx;
                    codeBuf[0] = 0;
                    codeBufIdx = mask = 1;
                }

                var lastMatchLength = _matchLength;
                for (i = 0; i < lastMatchLength && inputIdx < stopPos; i++)
                {
                    DeleteNode(s);
                    var c = input[inputIdx++];
                    TextBuffer[s] = c;
                    if (s < F - 1)
                    {
                        TextBuffer[s + N] = c;
                    }

                    s = (s + 1) & (N - 1);
                    r = (r + 1) & (N - 1);
                    InsertNode(r);
                }

                while (i++ < lastMatchLength)
                {
                    DeleteNode(s);
                    s = (s + 1) & (N - 1);
                    r = (r + 1) & (N - 1);
                    if (--len != 0)
                    {
                        InsertNode(r);
                    }
                }
            } while (len > 0);

            if (codeBufIdx > 1)
            {
                output.Write(codeBuf, 0, codeBufIdx);
                codeSize += codeBufIdx;
            }

            output.Flush();
            return codeSize;
        }
        /// <summary>
        ///     Initializes the binary search trees used by the compression algorithm.
        /// </summary>
        private static void InitTree()
        {
            int i;

            for (i = N + 1; i <= N + 256; i++)
            {
                NextChildren[i] = Nil;
            }

            for (i = 0; i < N; i++)
            {
                Parents[i] = Nil;
            }
        }
        /// <summary>
        ///     Inserts string of length F, TextBuffer[node..node+F-1], into one of the
        ///     trees (TextBuffer[node]'th tree) and returns the longest-match position
        ///     and length via the global variables MatchPosition and MatchLength.
        ///     If MatchLength = F, then removes the old node in favor of the new
        ///     one, because the old one will be deleted sooner.
        /// </summary>
        /// <param name="node">plays double role, as tree node and position in buffer.</param>
        private static void InsertNode(int node)
        {
            int cmp = 1, p = N + 1 + TextBuffer[node];
            _matchLength = 0;

            NextChildren[node] = PreviousChildren[node] = Nil;
            for (;;)
            {
                if (cmp >= 0)
                {
                    if (NextChildren[p] != Nil)
                    {
                        p = NextChildren[p];
                    }
                    else
                    {
                        NextChildren[p] = node;
                        Parents[node] = p;
                        return;
                    }
                }
                else
                {
                    if (PreviousChildren[p] != Nil)
                    {
                        p = PreviousChildren[p];
                    }
                    else
                    {
                        PreviousChildren[p] = node;
                        Parents[node] = p;
                        return;
                    }
                }

                int i;
                for (i = 1; i < F; i++)
                {
                    if ((cmp = TextBuffer[node + i] - TextBuffer[p + i]) != 0)
                    {
                        break;
                    }
                }

                if (i <= _matchLength)
                {
                    continue;
                }

                _matchPosition = p;
                if ((_matchLength = i) >= F)
                {
                    break;
                }
            }

            Parents[node] = Parents[p];
            PreviousChildren[node] = PreviousChildren[p];
            NextChildren[node] = NextChildren[p];
            Parents[PreviousChildren[p]] = node;
            Parents[NextChildren[p]] = node;
            if (NextChildren[Parents[p]] == p)
            {
                NextChildren[Parents[p]] = node;
            }
            else
            {
                PreviousChildren[Parents[p]] = node;
            }

            Parents[p] = Nil;
        }
        /// <summary>
        ///     Deletes node n from tree
        /// </summary>
        /// <param name="n">Node to remove from tree</param>
        private static void DeleteNode(int n)
        {
            int q;

            if (Parents[n] == Nil)
            {
                return;
            }

            if (NextChildren[n] == Nil)
            {
                q = PreviousChildren[n];
            }
            else if (PreviousChildren[n] == Nil)
            {
                q = NextChildren[n];
            }
            else
            {
                q = PreviousChildren[n];
                if (NextChildren[q] != Nil)
                {
                    do
                    {
                        q = NextChildren[q];
                    } while (NextChildren[q] != Nil);

                    NextChildren[Parents[q]] = PreviousChildren[q];
                    Parents[PreviousChildren[q]] = Parents[q];
                    PreviousChildren[q] = PreviousChildren[n];
                    Parents[PreviousChildren[n]] = q;
                }

                NextChildren[q] = NextChildren[n];
                Parents[NextChildren[n]] = q;
            }

            Parents[q] = Parents[n];
            if (NextChildren[Parents[n]] == n)
            {
                NextChildren[Parents[n]] = q;
            }
            else
            {
                PreviousChildren[Parents[n]] = q;
            }

            Parents[n] = Nil;
        }
    }
}