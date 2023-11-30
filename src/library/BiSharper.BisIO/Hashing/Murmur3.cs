﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Numerics.BitOperations;

namespace BiSharper.BisIO.Hashing;

public static class Murmur3
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Hash32(ReadOnlySpan<byte> bytes, uint seed)
    {
        ref var bp = ref MemoryMarshal.GetReference(bytes);
        ref var endPoint = ref Unsafe.Add(ref Unsafe.As<byte, uint>(ref bp), bytes.Length >> 2);
        if (bytes.Length >= 4)
        {
            do
            {
                seed = RotateLeft(seed ^ RotateLeft(Unsafe.ReadUnaligned<uint>(ref bp) * 3432918353U, 15) * 461845907U, 13) * 5 - 430675100;
                bp = ref Unsafe.Add(ref bp, 4);
            } while (Unsafe.IsAddressLessThan(ref Unsafe.As<byte, uint>(ref bp), ref endPoint));
        }

        var remainder = bytes.Length & 3;
        if (remainder > 0)
        {
            uint num = 0;
            if (remainder > 2) num ^= Unsafe.Add(ref endPoint, 2) << 16;
            if (remainder > 1) num ^= Unsafe.Add(ref endPoint, 1) << 8;
            num ^= endPoint;

            seed ^= RotateLeft(num * 3432918353U, 15) * 461845907U;
        }

        seed ^= (uint)bytes.Length;
        seed = (uint)((seed ^ (seed >> 16)) * -2048144789);
        seed = (uint)((seed ^ (seed >> 13)) * -1028477387);
        return seed ^ seed >> 16;
    }
    public static uint Hash32(ReadOnlyMemory<byte> memory, uint seed) =>
        Hash32(memory.Span, seed);
}