using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfWithCube
{
    public interface IWithCubeGroup
    {
    }

    public class WithCubeGroup<T> : IWithCubeGroup
    {
        public T Value { get; }

        public WithCubeGroup(T value)
        {
            Value = value;
        }
    }

    public class WithCubeRow<A>
    {
        public A Aggregate { get; set; }
    }

    public class WithCubeRow<G1, A> : WithCubeRow<A>
    {
        public WithCubeGroup<G1> Group1 { get; set; }
    }

    public class WithCubeRow<G1, G2, A> : WithCubeRow<G1, A>
    {
        public WithCubeGroup<G2> Group2 { get; set; }
    }

    public class WithCubeRow<G1, G2, G3, A> : WithCubeRow<G1, G2, A>
    {
        public WithCubeGroup<G3> Group3 { get; set; }
    }

    public class WithCubeRow<G1, G2, G3, G4, A> : WithCubeRow<G1, G2, G3, A>
    {
        public WithCubeGroup<G4> Group4 { get; set; }
    }
}
