using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarPuzzleSolver
{
    class Solver
    {
        enum Months
        {
            January = 0,
            February = 1,
            March = 2,
            April = 3,
            May = 4,
            June = 5,
            July = 8,
            August = 9,
            September = 10,
            October = 11,
            November = 12,
            December = 13
        }

        enum Weekdays
        {
            Sunday = 45,
            Monday = 46,
            Tuesday = 47,
            Wednesday = 48,
            Thursday = 53,
            Friday = 54,
            Saturday = 55
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Daily Calendar Solver!");
            Console.WriteLine("Example date format:  Saturday, October 30");
            Console.Write("Enter Date: ");

            var dateInput = Console.ReadLine();

            //TODO:  user input validation
            var dateArr = dateInput.Split(" ");
            var dayOfWeek = (Weekdays)Enum.Parse(typeof(Weekdays), dateArr[0].Substring(0, dateArr[0].IndexOf(',')));
            var month = (Months)Enum.Parse(typeof(Months), dateArr[1]);
            var dayOfMonth = Int32.Parse(dateArr[2]) + 13;

            //list to contain grid index values of the solution
            List<int> sols = new List<int>() { (int)dayOfWeek, (int)month, dayOfMonth };

            //create the ten 2-dimensional piece shapes
            var piece0 = new int[,] { { 1, 1, 1 }, { 1, 1, 0 } };
            var piece1 = new int[,] { { 1, 1, 1, 1 } };
            var piece2 = new int[,] { { 1, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };
            var piece3 = new int[,] { { 0, 1 }, { 0, 1 }, { 1, 1 }, { 1, 0 } };
            var piece4 = new int[,] { { 1, 0 }, { 1, 1 }, { 0, 1 } };
            var piece5 = new int[,] { { 0, 0, 1 }, { 0, 0, 1 }, { 1, 1, 1 } };
            var piece6 = new int[,] { { 1, 1, 1, 1 }, { 0, 0, 0, 1 } };
            var piece7 = new int[,] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 } };
            var piece8 = new int[,] { { 1, 1 }, { 1, 0 }, { 1, 0 } };
            var piece9 = new int[,] { { 1, 1 }, { 1, 0 }, { 1, 1 } };

            var pieces = new List<Piece>();//<int, Piece>();
            pieces.Add(new Piece(piece0, '0'));
            pieces.Add(new Piece(piece1, '1'));
            pieces.Add(new Piece(piece2, '2'));
            pieces.Add(new Piece(piece3, '3'));
            pieces.Add(new Piece(piece4, '4'));
            pieces.Add(new Piece(piece5, '5'));
            pieces.Add(new Piece(piece6, '6'));
            pieces.Add(new Piece(piece7, '7'));
            pieces.Add(new Piece(piece8, '8'));
            pieces.Add(new Piece(piece9, '9'));

            var calendar = new Calendar(sols);

            Console.Write("Solutions for date:"
                + Environment.NewLine
                + Environment.NewLine);

            //Call recursive function to find all solutions for the provided date
            FindAllSolutions(calendar, pieces);
        }

        public static void FindAllSolutions(Calendar calendar, List<Piece> remainingPieces)
        {
            //base case:  all Pieces were laid and solution was found
            if (remainingPieces.Count() == 0)
            {
                calendar.PrintCalendar();
            }

            //Attempt to lay each piece in the next available grid space
            foreach (Piece piece in remainingPieces)
            {
                var newCalendar = new Calendar(calendar.Sols, calendar);
                var validLay = false;

                foreach (int[,] shape in piece.Shapes)
                {
                    validLay = newCalendar.LayShape(shape, piece.Rep);

                    //if current piece shape fit in the space currently
                    //being filled, we can stop trying the other shapes
                    if (validLay) break;
                }

                //if no shape works for the current piece, try next piece
                if (!validLay) continue;

                var newRemainingPieces = new List<Piece>();
                foreach (Piece p in remainingPieces)
                {
                    newRemainingPieces.Add(p);
                }

                //remove the piece that was laid from the remaining pieces
                //to be placed and call the recursive function to try to
                //fit the rest of the pieces on the next empty grid space
                newRemainingPieces.Remove(piece);
                FindAllSolutions(newCalendar, newRemainingPieces);
            }
        }
    }
}
