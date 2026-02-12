using System.Collections.Generic;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockInt32Comparer : IComparer<int>
{
    int IComparer<int>.Compare(int x, int y) => y - x;
}
