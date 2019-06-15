using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinder
{

    using NodeList = List<Node>;
    using EdgeList = List<Edge>;
    using NodeWorkList = List<NodeWork>;
    using NodeWorkQueue = Queue<NodeWork>;

    /// <summary>
    /// 計算用のワーク
    /// </summary>
    class NodeWork
    {
        public Node node;
        public NodeWork from = null;
        public double cost = double.MaxValue;

        public NodeWork(Node _node) { node = _node; }
    };

    /// <summary>
    /// グラフを元に探索
    /// </summary>
    public class GraphSearch
    {
        // 探索用のワークリスト
        NodeWorkList m_calculateList = new NodeWorkList();
        // 最短経路
        NodeList m_shortestPaths = new NodeList();

        /// <summary>
        /// 経路探索
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        public void Search(Graph graph, int start, int goal)
        {
            // 探索候補のキュー
            NodeWorkQueue exploreQueue = new NodeWorkQueue();
            m_calculateList = new NodeWorkList();

            // 探索用のワークを生成
            foreach (var node in graph.GetNodes())
            {
                NodeWork newWork = new NodeWork(node);
                // 開始ノードの場合はコストを 0 にして探索キューに積む
                if (node.Id == start)
                {
                    newWork.cost = 0;
                    exploreQueue.Enqueue(newWork);
                }
                m_calculateList.Add(newWork);
            }

            // 探索開始
            while (0 < exploreQueue.Count)
            {
                NodeWork next = exploreQueue.Dequeue();

                // ゴールに到達
                if (next.node.Id == goal)
                {
                    break;
                }

                // 隣接ノードのコストを計算
                foreach (var linkedEdge in graph.GetTransitableEdges(next.node.Id))
                {
                    double newCost = m_calculateList.Find((_nodeCost) => _nodeCost.node.Id == next.node.Id).cost + linkedEdge.Cost;

                    NodeWork toNodeCost = m_calculateList.Find((_work) => _work.node.Id == linkedEdge.To);
                    if (newCost < toNodeCost.cost)
                    {
                        toNodeCost.cost = newCost;
                        toNodeCost.from = m_calculateList.Find((_work) => _work.node.Id == linkedEdge.From);
                        exploreQueue.Enqueue(toNodeCost);
                    }
                }
            }

            // 最短経路を保存
            m_shortestPaths = CalcShortestPaths(goal);
        }

        /// <summary>
        /// 最短経路を取得
        /// </summary>
        /// <returns></returns>
        public NodeList GetShortestPaths()
        {
            return m_shortestPaths;
        }

        /// <summary>
        /// 探索結果から最短経路を獲得
        /// </summary>
        /// <returns></returns>
        public NodeList CalcShortestPaths(int goal)
        {
            NodeList shortestPath = new NodeList();
            NodeWork cur = m_calculateList.Find((_work) => _work.node.Id == goal);
            while (cur != null)
            {
                shortestPath.Add(cur.node);
                cur = cur.from;
            }
            shortestPath.Reverse();
            return shortestPath;
        }

        public void Test()
        {
            Graph graph = new Graph();
            graph.AddNode(new Node(0));
            graph.AddNode(new Node(1));
            graph.AddNode(new Node(2));
            graph.AddNode(new Node(3));
            graph.AddNode(new Node(4));
            graph.AddNode(new Node(5));

            graph.AddEdge(new Edge(0, 1, 1.0));
            graph.AddEdge(new Edge(1, 2, 5.0));
            graph.AddEdge(new Edge(1, 3, 1.0));
            graph.AddEdge(new Edge(3, 2, 2.0));
            graph.AddEdge(new Edge(2, 4, 1.0));
            graph.AddEdge(new Edge(2, 5, 5.0));
            graph.AddEdge(new Edge(4, 5, 1.0));

            Search(graph, 0, 5);

            var sb = new System.Text.StringBuilder();
            foreach (var node in m_shortestPaths)
            {
                sb.Append(node.Id + " -> ");
            }
            Debug.Log(sb.ToString());
        }
    }

    /// <summary>
    /// ノードとエッジを管理するグラフ
    /// </summary>
    public class Graph
    {
        NodeList Nodes { get; } = new NodeList();
        EdgeList Edges { get; } = new EdgeList();

        public void AddNode(Node node)
        {
            if (Nodes.Exists((_item) => _item.Id == node.Id))
            {
                return;
            }
            Nodes.Add(node);
        }
        public Node GetNode(int id) { return Nodes.Find((data) => data.Id == id); }
        public IEnumerable<Node> GetNodes() { return Nodes; }

        public void AddEdge(Edge edge) { Edges.Add(edge); }
        public Edge GetEdge(int from, int to) { return Edges.Find((data) => data.From == from && data.To == to); }
        public IEnumerable<Edge> GetEdges() { return Edges; }
        public IEnumerable<Edge> GetTransitableEdges(int id) { return Edges.Where((_edge) => _edge.From == id); }
    }

    /// <summary>
    /// ノード
    /// </summary>
    public class Node
    {
        public int Id { get; set; }
        public object UserData { get; set; } = null;

        public Node(int _id) { Id = _id; }
        public Node(int _id, object _userData) { Id = _id; UserData = _userData; }
    }

    /// <summary>
    /// エッジ
    /// </summary>
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public double Cost { get; set; }

        public Edge(int _from, int _to, double _cost) { From = _from; To = _to; Cost = _cost; }
    }
}
