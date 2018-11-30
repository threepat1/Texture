using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace Tetris
{
    public class Grid : MonoBehaviour
    {
        #region Singleton
        public static Grid Instance = null;
        private void Awake()
        {
            Instance = this;
        }
        private void OnDestroy()
        {
            Instance = null;
        }
        #endregion

        public int width = 10, height = 20;
        public Transform[,] data;
        
        public delegate void RowsClearedCallback(int rows);
        public RowsClearedCallback onRowsCleared;

        private void Start()
        {
            data = new Transform[width, height];
        }

        void OnDrawGizmos()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.DrawWireCube(new Vector3(x, y), Vector2.one);
                }
            }
        }
        
        // Checks if position is within border
        public bool InsideBorder(Vector2 pos)
        {
            // Truncate the vector
            int x = (int)pos.x;
            int y = (int)pos.y;
            
            // Is the position within the bounds of the grid?
            if(x >= 0 && x < width &&
               y >= 0) // Do not need to check height
            {
                // Inside border
                return true;
            }
            // Outside border
            return false;
        }
        // Deletes a row with a given y coordinate
        public void DeleteRow(int y)
        {
            // Loop through the column using x - width
            for (int x = 0; x < width; x++)
            {
                // Destroy each element
                Destroy(data[x, y].gameObject);
                // Return each grid element back to null
                data[x, y] = null;
            }
        }
        // Shifts the row in the y coordinate down one space
        public void DecreaseRow(int y)
        {
            // Loop through entire column
            for (int x = 0; x < width; x++)
            {
                // Check if index is not null
                if(data[x, y] != null)
                {
                    // Move one towards bottom
                    data[x, y - 1] = data[x, y]; // Set grid element to one above
                    data[x, y] = null;
                    // Update block position
                    data[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
        // Shifts rows above from given y
        public void DecreaseRowsAbove(int y)
        {
            // Loop through each row
            for (int i = y; i < height; i++)
            {
                // Decrease each row
                DecreaseRow(i);
            }
        }
        // Check if we have a full row
        public bool IsRowFull(int y)
        {
            // Loop through each column
            for (int x = 0; x < width; x++)
            {
                // If cell is empty
                if (data[x, y] == null)
                    return false;
            }
            // The row is full!
            return true;
        }
        // Delete the full rows
        public int DeleteFullRows()
        {
            int rows = 0;
            // Loop through all rows
            for (int y = 0; y < height; y++)
            {
                // Is the row full?
                if(IsRowFull(y))
                {
                    // Add row to count
                    rows++;
                    // Delete entire row
                    DeleteRow(y);
                    // Decrease the rows above
                    DecreaseRowsAbove(y + 1);
                    // Decrease current y coordinate 
                    // (so we don't skip the next row)
                    y--;
                }
            }

            // If there are rows that were cleared AND
            // Functions are subscribed to onRowsCleared
            if(rows > 0 && Instance.onRowsCleared != null)
            {
                // Invoke all the subscribed functions
                Instance.onRowsCleared.Invoke(rows);
            }

            // Return counted rows
            return rows;
        }
        // Rounds Vector2 to nearest whole number
        public Vector2 RoundVec2(Vector2 v)
        {
            float roundX = Mathf.Round(v.x);
            float roundY = Mathf.Round(v.y);
            return new Vector2(roundX, roundY);
        }
    }
}