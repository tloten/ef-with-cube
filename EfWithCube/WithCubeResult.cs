using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfWithCube
{
    public abstract class WithCubeResultBase<G, A> where G : WithCubeRow<A>
    {
        public IList<G> AllRows { get; set; }
        protected Lazy<A> lazyGrandTotal;
        public A GrandTotal { get { return lazyGrandTotal.Value; } }

        protected Dictionary<T, A> CalculateTotals<T>(Func<G, WithCubeGroup<T>> desiredGroupSelector, Func<G, IWithCubeGroup> undesiredGroupSelector)
        {
            return AllRows
                .Where(r => desiredGroupSelector(r) != null && undesiredGroupSelector(r) == null)
                .ToDictionary(r => desiredGroupSelector(r).Value, r => r.Aggregate);
        }
    }

    public class WithCubeResult<G1, A> : WithCubeResultBase<WithCubeRow<G1, A>, A>
    {
        private Lazy<Dictionary<G1, A>> lazyGroup1;

        public Dictionary<G1, A> Group1 { get { return lazyGroup1.Value; } }

        public WithCubeResult()
        {
            lazyGroup1 = new Lazy<Dictionary<G1, A>>(() => CalculateTotals((r) => r.Group1, (r) => null));
            lazyGrandTotal = new Lazy<A>(() => AllRows.Where(r => r.Group1 == null).FirstOrDefault().Aggregate);
        }
    }

    public class WithCubeResult<G1, G2, A> : WithCubeResultBase<WithCubeRow<G1, G2, A>, A>
    {
        private Lazy<Dictionary<G1, A>> lazyGroup1;
        private Lazy<Dictionary<G2, A>> lazyGroup2;
        
        public Dictionary<G1, A> Group1 { get { return lazyGroup1.Value; } }
        public Dictionary<G2, A> Group2 { get { return lazyGroup2.Value; } }

        public WithCubeResult()
        {
            lazyGroup1 = new Lazy<Dictionary<G1, A>>(() => CalculateTotals((r) => r.Group1, (r) => r.Group2));
            lazyGroup2 = new Lazy<Dictionary<G2, A>>(() => CalculateTotals((r) => r.Group2, (r) => r.Group1));
            lazyGrandTotal = new Lazy<A>(() => AllRows.Where(r => r.Group1 == null && r.Group2 == null).Select(r => r.Aggregate).FirstOrDefault());
        }        
    }

    public class WithCubeResult<G1, G2, G3, A> : WithCubeResultBase<WithCubeRow<G1, G2, G3, A>, A>
    {
        private Lazy<Dictionary<G1, A>> lazyGroup1;
        private Lazy<Dictionary<G2, A>> lazyGroup2;
        private Lazy<Dictionary<G3, A>> lazyGroup3;
        
        public Dictionary<G1, A> Group1 { get { return lazyGroup1.Value; } }
        public Dictionary<G2, A> Group2 { get { return lazyGroup2.Value; } }
        public Dictionary<G3, A> Group3 { get { return lazyGroup3.Value; } }

        public WithCubeResult()
        {
            lazyGroup1 = new Lazy<Dictionary<G1, A>>(() => CalculateTotals((r) => r.Group1, (r) => r.Group2 ?? (IWithCubeGroup)r.Group3));
            lazyGroup2 = new Lazy<Dictionary<G2, A>>(() => CalculateTotals((r) => r.Group2, (r) => r.Group1 ?? (IWithCubeGroup)r.Group3));
            lazyGroup3 = new Lazy<Dictionary<G3, A>>(() => CalculateTotals((r) => r.Group3, (r) => r.Group1 ?? (IWithCubeGroup)r.Group2));
            lazyGrandTotal = new Lazy<A>(() => AllRows.Where(r => r.Group1 == null && r.Group2 == null && r.Group3 == null).FirstOrDefault().Aggregate);
        }
    }


    public class WithCubeResult<G1, G2, G3, G4, A> : WithCubeResultBase<WithCubeRow<G1, G2, G3, G4, A>, A>
    {
        private Lazy<Dictionary<G1, A>> lazyGroup1;
        private Lazy<Dictionary<G2, A>> lazyGroup2;
        private Lazy<Dictionary<G3, A>> lazyGroup3;
        private Lazy<Dictionary<G4, A>> lazyGroup4;
        
        public Dictionary<G1, A> Group1 { get { return lazyGroup1.Value; } }
        public Dictionary<G2, A> Group2 { get { return lazyGroup2.Value; } }
        public Dictionary<G3, A> Group3 { get { return lazyGroup3.Value; } }
        public Dictionary<G4, A> Group4 { get { return lazyGroup4.Value; } }

        public WithCubeResult()
        {
            lazyGroup1 = new Lazy<Dictionary<G1, A>>(() => CalculateTotals((r) => r.Group1, (r) => r.Group2 ?? r.Group3 ?? (IWithCubeGroup)r.Group4));
            lazyGroup2 = new Lazy<Dictionary<G2, A>>(() => CalculateTotals((r) => r.Group2, (r) => r.Group1 ?? r.Group3 ?? (IWithCubeGroup)r.Group4));
            lazyGroup3 = new Lazy<Dictionary<G3, A>>(() => CalculateTotals((r) => r.Group3, (r) => r.Group1 ?? r.Group2 ?? (IWithCubeGroup)r.Group4));
            lazyGroup4 = new Lazy<Dictionary<G4, A>>(() => CalculateTotals((r) => r.Group4, (r) => r.Group1 ?? r.Group2 ?? (IWithCubeGroup)r.Group3));
            lazyGrandTotal = new Lazy<A>(() => AllRows.Where(r => r.Group1 == null && r.Group2 == null && r.Group3 == null && r.Group4 == null).FirstOrDefault().Aggregate);
        }
    }
}
