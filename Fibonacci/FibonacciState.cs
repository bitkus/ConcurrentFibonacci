using System.Collections.Generic;
using System.Linq;

namespace Fibonacci
{
    public class FibonacciState
    {
        private readonly List<FibonacciStateItem> _fibonacciList;
        private static readonly object Lock = new object();
        private readonly Dictionary<string, bool> _registeredNodes;

        public bool IsEveryNodeReady => _registeredNodes.All(n => n.Value);

        public FibonacciState()
        {
            _fibonacciList = new List<FibonacciStateItem>();
            _registeredNodes = new Dictionary<string, bool>();
        }

        public bool TryGetAndGrabNextAvailableIndex(string nodeId, out int index)
        {
            lock(Lock) 
            {
                var max = _fibonacciList.OrderBy(f => f.Index).Last().Index;
                var min = _fibonacciList.OrderBy(f => f.Index).First().Index;

                for (var i = min; i <= max; i++)
                {
                    if (!IsPreviousIndexTakenByThisNode(i, nodeId) && !DoesIndexStateItemExist(i))
                    {
                        _fibonacciList.Add(new FibonacciStateItem
                        {
                            Fibonacci = 0,
                            Index = i,
                            NodeId = nodeId,
                            ComputationState = ComputationState.Computing
                        });

                        index = i;
                        return true;
                    }
                }

                index = int.MinValue;
                return false;
            }
        }

        public void RegisterNode(string nodeId) => _registeredNodes.Add(nodeId, false);

        public void NoteNodeReady(string nodeId) => _registeredNodes[nodeId] = true;

        private bool IsPreviousIndexTakenByThisNode(int index, string thisNodeId)
        {
            var state = _fibonacciList.FirstOrDefault(f => f.Index == index - 1);
            return state != null && state.NodeId == thisNodeId;
        }

        private bool DoesIndexStateItemExist(int index)
        {
            return _fibonacciList.Any(f => f.Index == index);
        }

        public void Update(int index, long fibonacci)
        {
            lock (Lock)
            {
                var item = _fibonacciList.First(f => f.Index == index);
                item.Fibonacci = fibonacci;
                item.ComputationState = ComputationState.Computed;
            }
        }

        public bool IsFibonacciMissing(int index)
        {
            lock (Lock)
            {
                return _fibonacciList.OrderBy(f => f.Index).First().Index - 1 == index;
            }
        }

        public bool TryGet(int index, out long fibonacci)
        {
            lock (Lock)
            {
                var stateItem = _fibonacciList.FirstOrDefault(f => f.Index == index);
                fibonacci = stateItem?.Fibonacci ?? long.MinValue;
                return stateItem != null && stateItem.ComputationState == ComputationState.Computed;
            }
        }

        public void Add(FibonacciStateItem stateItem)
        {
            lock (Lock)
            {
                _fibonacciList.Add(stateItem);
            }
        }
    }
}