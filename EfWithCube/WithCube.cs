using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace EfWithCube
{
    public static class Extensions
    {
        public static WithCubeResult<G1, A> WithCube<T, G1, A>(this IQueryable<T> queryable, Expression<Func<T, G1>> group1Selector, Expression<Func<IEnumerable<T>, A>> aggregate)
        {
            queryable = queryable.AsExpandable();

            var qg1 = queryable
                .GroupBy(e => new { Group1 = group1Selector.Invoke(e) })
                .Select(g => new
                {
                    g.Key.Group1,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1
                });

            var qg0 = queryable
                .GroupBy(g => true)
                .Select(g => new
                {
                    Group1 = default(G1),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.None
                });

            var q = qg1.Union(qg0).ToList();

            return new WithCubeResult<G1, A>
            {
                AllRows = q.Select(r => new WithCubeRow<G1, A>
                {
                    Group1 = (r.Groups & SpecifiedGroups.Group1) == SpecifiedGroups.Group1 ? new WithCubeGroup<G1>(r.Group1) : null,
                    Aggregate = r.Aggregate
                })
                .ToList()
            };
        }

        public static WithCubeResult<G1, G2, A> WithCube<T, G1, G2, A>(this IQueryable<T> queryable, Expression<Func<T, G1>> group1Selector, Expression<Func<T, G2>> group2Selector, Expression<Func<IEnumerable<T>, A>> aggregate)
        {
            queryable = queryable.AsExpandable();

            var qg1 = queryable
                .GroupBy(e => new { Group1 = group1Selector.Invoke(e) })
                .Select(g => new
                {
                    g.Key.Group1,
                    Group2 = default(G2),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1
                });

            var qg12 = queryable
                   .GroupBy(e => new { Group1 = group1Selector.Invoke(e), Group2 = group2Selector.Invoke(e) })
                   .Select(g => new
                   {
                       g.Key.Group1,
                       g.Key.Group2,
                       Aggregate = aggregate.Invoke(g),
                       Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group2
                   });

            var qg2 = queryable
                  .GroupBy(e => new { Group2 = group2Selector.Invoke(e) })
                  .Select(g => new
                  {
                      Group1 = default(G1),
                      g.Key.Group2,
                      Aggregate = aggregate.Invoke(g),
                      Groups = SpecifiedGroups.Group2
                  });

            var qg0 = queryable
                .GroupBy(g => true)
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = default(G2),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.None
                });

            var q = qg1.Union(qg12).Union(qg2).Union(qg0).ToList();

            return new WithCubeResult<G1, G2, A>
            {
                AllRows = q.Select(r => new WithCubeRow<G1, G2, A>
                {
                    Group1 = (r.Groups & SpecifiedGroups.Group1) == SpecifiedGroups.Group1 ? new WithCubeGroup<G1>(r.Group1) : null,
                    Group2 = (r.Groups & SpecifiedGroups.Group2) == SpecifiedGroups.Group2 ? new WithCubeGroup<G2>(r.Group2) : null,
                    Aggregate = r.Aggregate
                })
                .ToList()
            };
        }

        public static WithCubeResult<G1, G2, G3, A> WithCube<T, G1, G2, G3, A>(this IQueryable<T> queryable, Expression<Func<T, G1>> group1Selector, Expression<Func<T, G2>> group2Selector, Expression<Func<T, G3>> group3Selector, Expression<Func<IEnumerable<T>, A>> aggregate)
        {
            queryable = queryable.AsExpandable();

            var qg123 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group2 = group2Selector.Invoke(g), Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = g.Key.Group2,
                    Group3 = g.Key.Group3,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group2 | SpecifiedGroups.Group3
                });

            var qg1 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = default(G2),
                    Group3 = default(G3),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1
                });

            var qg12 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group2 = group2Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = g.Key.Group2,
                    Group3 = default(G3),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group2
                });

            var qg13 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = default(G2),
                    Group3 = g.Key.Group3,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group3
                });

            var qg2 = queryable
                .GroupBy(g => new { Group2 = group2Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = g.Key.Group2,
                    Group3 = default(G3),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group2
                });

            var qg23 = queryable
                .GroupBy(g => new { Group2 = group2Selector.Invoke(g), Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = g.Key.Group2,
                    Group3 = g.Key.Group3,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group2 | SpecifiedGroups.Group3
                });

            var qg3 = queryable
                .GroupBy(g => new { Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = default(G2),
                    Group3 = g.Key.Group3,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group3
                });

            var qg0 = queryable
                .GroupBy(g => true)
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = default(G2),
                    Group3 = default(G3),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.None
                });

            var q = qg123.Union(qg1).Union(qg12).Union(qg13).Union(qg2).Union(qg23).Union(qg3).Union(qg0).ToList();

            return new WithCubeResult<G1, G2, G3, A>
            {
                AllRows = q.Select(r => new WithCubeRow<G1, G2, G3, A>
                {
                    Group1 = (r.Groups & SpecifiedGroups.Group1) == SpecifiedGroups.Group1 ? new WithCubeGroup<G1>(r.Group1) : null,
                    Group2 = (r.Groups & SpecifiedGroups.Group2) == SpecifiedGroups.Group2 ? new WithCubeGroup<G2>(r.Group2) : null,
                    Group3 = (r.Groups & SpecifiedGroups.Group3) == SpecifiedGroups.Group3 ? new WithCubeGroup<G3>(r.Group3) : null,
                    Aggregate = r.Aggregate
                })
                .ToList()
            };
        }

        public static WithCubeResult<G1, G2, G3, G4, A> WithCube<T, G1, G2, G3, G4, A>(this IQueryable<T> queryable, Expression<Func<T, G1>> group1Selector, Expression<Func<T, G2>> group2Selector, Expression<Func<T, G3>> group3Selector, Expression<Func<T, G4>> group4Selector, Expression<Func<IEnumerable<T>, A>> aggregate)
        {
            queryable = queryable.AsExpandable();

            var qg1 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = default(G2),
                    Group3 = default(G3),
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1
                });

            var qg12 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group2 = group2Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = g.Key.Group2,
                    Group3 = default(G3),
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group2
                });

            var qg123 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group2 = group2Selector.Invoke(g), Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = g.Key.Group2,
                    Group3 = g.Key.Group3,
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group2 | SpecifiedGroups.Group3
                });

            var qg1234 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group2 = group2Selector.Invoke(g), Group3 = group3Selector.Invoke(g), Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = g.Key.Group2,
                    Group3 = g.Key.Group3,
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group2 | SpecifiedGroups.Group3 | SpecifiedGroups.Group4
                });

            var qg124 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group2 = group2Selector.Invoke(g), Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = g.Key.Group2,
                    Group3 = default(G3),
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group2 | SpecifiedGroups.Group4
                });

            var qg13 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = default(G2),
                    Group3 = g.Key.Group3,
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group3
                });

            var qg134 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group3 = group3Selector.Invoke(g), Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = default(G2),
                    Group3 = g.Key.Group3,
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group3 | SpecifiedGroups.Group4
                });

            var qg14 = queryable
                .GroupBy(g => new { Group1 = group1Selector.Invoke(g), Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = g.Key.Group1,
                    Group2 = default(G2),
                    Group3 = default(G3),
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group1 | SpecifiedGroups.Group4
                });

            var qg2 = queryable
                .GroupBy(g => new { Group2 = group2Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = g.Key.Group2,
                    Group3 = default(G3),
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group2
                });

            var qg23 = queryable
                .GroupBy(g => new { Group2 = group2Selector.Invoke(g), Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = g.Key.Group2,
                    Group3 = g.Key.Group3,
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group2 | SpecifiedGroups.Group3
                });

            var qg234 = queryable
                .GroupBy(g => new { Group2 = group2Selector.Invoke(g), Group3 = group3Selector.Invoke(g), Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = g.Key.Group2,
                    Group3 = g.Key.Group3,
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group2 | SpecifiedGroups.Group3 | SpecifiedGroups.Group4
                });


            var qg24 = queryable
                .GroupBy(g => new { Group2 = group2Selector.Invoke(g), Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = g.Key.Group2,
                    Group3 = default(G3),
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group2 | SpecifiedGroups.Group4
                });


            var qg3 = queryable
                .GroupBy(g => new { Group3 = group3Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = default(G2),
                    Group3 = g.Key.Group3,
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group3
                });

            var qg34 = queryable
                .GroupBy(g => new { Group3 = group3Selector.Invoke(g), Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = default(G2),
                    Group3 = g.Key.Group3,
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group3 | SpecifiedGroups.Group4
                });

            var qg4 = queryable
                .GroupBy(g => new { Group4 = group4Selector.Invoke(g) })
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = default(G2),
                    Group3 = default(G3),
                    Group4 = g.Key.Group4,
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.Group4
                });

            var qg0 = queryable
                .GroupBy(g => true)
                .Select(g => new
                {
                    Group1 = default(G1),
                    Group2 = default(G2),
                    Group3 = default(G3),
                    Group4 = default(G4),
                    Aggregate = aggregate.Invoke(g),
                    Groups = SpecifiedGroups.None
                });

            var q = qg1.Union(qg12).Union(qg123).Union(qg1234).Union(qg124).Union(qg13).Union(qg134).Union(qg14)
                .Union(qg2).Union(qg23).Union(qg234).Union(qg24)
                .Union(qg3).Union(qg34)
                .Union(qg4)
                .Union(qg0)
                .ToList();

            return new WithCubeResult<G1, G2, G3, G4, A>
            {
                AllRows = q.Select(r => new WithCubeRow<G1, G2, G3, G4, A>
                {
                    Group1 = (r.Groups & SpecifiedGroups.Group1) == SpecifiedGroups.Group1 ? new WithCubeGroup<G1>(r.Group1) : null,
                    Group2 = (r.Groups & SpecifiedGroups.Group2) == SpecifiedGroups.Group2 ? new WithCubeGroup<G2>(r.Group2) : null,
                    Group3 = (r.Groups & SpecifiedGroups.Group3) == SpecifiedGroups.Group3 ? new WithCubeGroup<G3>(r.Group3) : null,
                    Group4 = (r.Groups & SpecifiedGroups.Group4) == SpecifiedGroups.Group4 ? new WithCubeGroup<G4>(r.Group4) : null,
                    Aggregate = r.Aggregate
                })
                .ToList()
            };
        }

        [Flags]
        private enum SpecifiedGroups
        {
            None = 0,
            Group1 = 1,
            Group2 = 2,
            Group3 = 4,
            Group4 = 8
        }
    }
}
