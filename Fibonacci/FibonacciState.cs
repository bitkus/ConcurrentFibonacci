using System.Collections.Generic;
using System.Linq;

namespace Fibonacci
{
    public class FibonacciState
    {
        private readonly List<FibonacciStateItem> _fibonacciList;
        private static readonly object Lock = new object();

        public FibonacciState()
        {
            _fibonacciList = new List<FibonacciStateItem>();
        }

        public bool TryGetAndGrabFirstNotComputedIndex(string nodeId, out FibonacciStateItem stateItem)
        {
            lock(Lock) 
            {
                var max = _fibonacciList.OrderBy(f => f.Index).Last().Index;
                var min = _fibonacciList.OrderBy(f => f.Index).First().Index;

                for (var i = min; i <= max; i++)
                {
                    if (!IsIndexComputed(i))
                    {
                        stateItem = new FibonacciStateItem
                        {
                            Fibonacci = 0,
                            Index = i,
                            NodeId = nodeId,
                            ComputationState = ComputationState.Computing
                        };
                        _fibonacciList.Add(stateItem);

                        return true;
                    }
                }

                stateItem = default;
                return false;
            }
        }

        private bool IsIndexComputed(int index)
        {
            return _fibonacciList.Any(f => f.Index == index && f.ComputationState == ComputationState.Computed);
        }

        public void Update(FibonacciStateItem stateItem)
        {
            lock (Lock)
            {
                var item = _fibonacciList.First(f => f.Index == stateItem.Index);
                item.Fibonacci = stateItem.Fibonacci;
                item.ComputationState = ComputationState.Computed;
            }
        }

        public bool TryGet(int index, string nodeId, out FibonacciStateItem stateItem)
        {
            lock (Lock)
            {
                stateItem = IsComputedOrBeingComputedByOtherNode(index, nodeId);
                return stateItem != null;
            }
        }

        public void Add(FibonacciStateItem stateItem)
        {
            lock (Lock)
            {
                _fibonacciList.Add(stateItem);
            }
        }

        private FibonacciStateItem IsComputedOrBeingComputedByOtherNode(int index, string nodeId)
        {
            //return _fibonacciList.FirstOrDefault(f => f.Index == index &&
            //                                          f.ComputationState == ComputationState.Computed ||
            //                                          (f.ComputationState == ComputationState.Computing &&
            //                                           f.NodeId == nodeId));
            return _fibonacciList.FirstOrDefault(f => f.Index == index && 
                                                      f.ComputationState != ComputationState.NotComputed);
        }
    }
}