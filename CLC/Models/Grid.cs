/*
 * Grid.cs - Grid model class.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using System;

namespace CLC.Models
{
    public class Grid
    {
        // Declare the variables.
        private int id;
        private int rows;
        private int cols;
        private int userid;
        private Boolean gameOver;
        private Cell[,] cells;

        // Create the constructors.
        public Grid()
        {

        }
        public Grid(int id, int rows, int cols, int userid, bool gameOver)
        {
            this.id = id;
            this.rows = rows;
            this.cols = cols;
            this.userid = userid;
            this.gameOver = gameOver;
        }

        // Create the public getters and setters.
        public int Id { get => id; set => id = value; }
        public int Rows { get => rows; set => rows = value; }
        public int Cols { get => cols; set => cols = value; }
        public int Userid { get => userid; set => userid = value; }
        public bool GameOver { get => gameOver; set => gameOver = value; }
        public Cell[,] Cells { get => cells; set => cells = value; }

    }
}