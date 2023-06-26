/*
 * SavedGame.cs - SavedGame model class.
 * Authors - Martin, Ryan, and Raymond
 * Date - 06/25/2023
 */
using System;
using System.ComponentModel;

namespace CLC.Models
{
    public class SavedGame
    {
        //Declare the variables and create public getters and setters.
        [DisplayName("Id number")]
        public int id { get; set; }
        public int userId { get; set; }
        public int gridId { get; set; }
        public int rows { get; set; }
        public int cols { get; set; }
        public DateTime date { get; set; }

        // Create the constructors.
        public SavedGame()
        {

        }
        public SavedGame(int id, int userId, int gridId, int rows, int cols, DateTime date)
        {
            this.id = id;
            this.userId = userId;
            this.gridId = gridId;
            this.rows = rows;
            this.cols = cols;
            this.date = date;
        }
    }
}