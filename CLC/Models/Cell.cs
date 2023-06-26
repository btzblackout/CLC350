/*
 * Cell.cs - Cell model class.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using System;

namespace CLC.Models
{
    public class Cell
    {
        // Declare variables.
        private int id;
        private int x;
        private int y;
        private int liveNeighbors;
        private Boolean visited;
        private Boolean bomb;
        private Boolean isFlagged;

        // Create constructors.
        public Cell()
        {
            id = -1;
            x = -1;
            y = -1;
            liveNeighbors = 0;
            visited = false;
            bomb = false;
            isFlagged = false;
        }

        public Cell(int x, int y)
        {
            id = -1;
            this.x = x;
            this.y = y;
            liveNeighbors = 0;
            visited = false;
            bomb = false;
            isFlagged = false;
        }

        // Public getters and setters.
        public int Id { get => id; set => id = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int LiveNeighbors { get => liveNeighbors; set => liveNeighbors = value; }
        public bool Visited { get => visited; set => visited = value; }
        public bool Bomb { get => bomb; set => bomb = value; }
        public bool IsFlagged { get => isFlagged; set => isFlagged = value; }   
    }
}