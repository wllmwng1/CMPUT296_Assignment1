using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Assignment 1 Part A Script
public class GridHandler : NodeHandler
{
    //Size of grid points
    private float gridSize = 0.2f;

    //Holds all of the nodes
    private Dictionary<string, GraphNode> nodeDictionary;
    public override void CreateNodes()
    {
        nodeDictionary = new Dictionary<string, GraphNode>();

        //ASSIGNMENT 1 EDIT BELOW THIS LINE

        for (float x = ObstacleHandler.Instance.Width * -1; x <= ObstacleHandler.Instance.Width + gridSize; x += gridSize)
        {
            for (float y = ObstacleHandler.Instance.Height * -1; y <= ObstacleHandler.Instance.Height + gridSize; y += gridSize)
            {
                Vector3 loc = new Vector3(x, y);
                //Check validity
                //1st check: Obstacle on loc
                if (!ObstacleHandler.Instance.PointInObstacles(loc)
                    & !ObstacleHandler.Instance.AnyIntersect(loc + Vector3.left * gridSize / 2f + Vector3.up * gridSize / 2f, loc + Vector3.left * gridSize / 2f + Vector3.down * gridSize / 2f)
                    & !ObstacleHandler.Instance.AnyIntersect(loc + Vector3.left * gridSize / 2f + Vector3.up * gridSize / 2f, loc + Vector3.right * gridSize / 2f + Vector3.up * gridSize / 2f)
                    & !ObstacleHandler.Instance.AnyIntersect(loc + Vector3.right * gridSize / 2f + Vector3.up * gridSize / 2f, loc + Vector3.right * gridSize / 2f + Vector3.down * gridSize / 2f)
                    & !ObstacleHandler.Instance.AnyIntersect(loc + Vector3.right * gridSize / 2f + Vector3.down * gridSize / 2f, loc + Vector3.left * gridSize / 2f + Vector3.down * gridSize / 2f))
                {
                    Bounds bbox = new Bounds(loc, new Vector3(gridSize, gridSize));
                    bool inside = false;
                    foreach (Vector3 point in ObstacleHandler.Instance.GetObstaclePoints())
                    {
                        if (bbox.Contains(point))
                        {
                            inside = true;
                            break;
                        }
                    }
                    if (!inside)
                    {
                        nodeDictionary.Add(loc.ToString(), new GraphNode(loc));
                    }
                }
            }
        }
        
        //ASSIGNMENT 1 EDIT ABOVE THIS LINE

        //Create Neighbors
        foreach (KeyValuePair<string, GraphNode> kvp in nodeDictionary)
        {
            //Left
            if (nodeDictionary.ContainsKey((kvp.Value.Location + (Vector3.left * gridSize)).ToString()))
            {
                kvp.Value.AddNeighbor(nodeDictionary[(kvp.Value.Location + (Vector3.left * gridSize)).ToString()]);
            }
            //Right
            if (nodeDictionary.ContainsKey((kvp.Value.Location + (Vector3.right * gridSize)).ToString()))
            {
                kvp.Value.AddNeighbor(nodeDictionary[(kvp.Value.Location + (Vector3.right * gridSize)).ToString()]);
            }
            //Up
            if (nodeDictionary.ContainsKey((kvp.Value.Location + (Vector3.up * gridSize)).ToString()))
            {
                kvp.Value.AddNeighbor(nodeDictionary[(kvp.Value.Location + (Vector3.up * gridSize)).ToString()]);
            }
            //Down
            if (nodeDictionary.ContainsKey((kvp.Value.Location + (Vector3.down * gridSize)).ToString()))
            {
                kvp.Value.AddNeighbor(nodeDictionary[(kvp.Value.Location + (Vector3.down * gridSize)).ToString()]);
            }
        }
    }

    public override void VisualizeNodes()
    {
        //Visualize grid points
        foreach (KeyValuePair<string, GraphNode> kvp in nodeDictionary)
        {
            //Draw left line
            Debug.DrawLine(kvp.Value.Location + Vector3.left * gridSize / 2f + Vector3.up * gridSize / 2f, kvp.Value.Location + Vector3.left * gridSize / 2f + Vector3.down * gridSize / 2f, Color.white);
            //Draw right line
            Debug.DrawLine(kvp.Value.Location + Vector3.right * gridSize / 2f + Vector3.up * gridSize / 2f, kvp.Value.Location + Vector3.right * gridSize / 2f + Vector3.down * gridSize / 2f, Color.white);
            //Draw top line
            Debug.DrawLine(kvp.Value.Location + Vector3.up * gridSize / 2f + Vector3.left * gridSize / 2f, kvp.Value.Location + Vector3.up * gridSize / 2f + Vector3.right * gridSize / 2f, Color.white);
            //Draw bottom line
            Debug.DrawLine(kvp.Value.Location + Vector3.down * gridSize / 2f + Vector3.left * gridSize / 2f, kvp.Value.Location + Vector3.down * gridSize / 2f + Vector3.right * gridSize / 2f, Color.white);
        }
    }

    //Find closest node (used for pathing)
    public override GraphNode ClosestNode(Vector3 position)
    {
        float minDist = 1000;
        GraphNode closest = null;
        foreach (KeyValuePair<string, GraphNode> kvp in nodeDictionary)
        {
            float dist = (kvp.Value.Location - position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = kvp.Value;
            }
        }
        return closest;
    }
}
