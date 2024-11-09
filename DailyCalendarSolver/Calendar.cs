using System;
using System.Collections.Generic;

namespace CalendarPuzzleSolver
{
    class Calendar
    {
        const int Height = 8;
        const int Width = 7;
        public char[] Values { get; set; }
        public List<int> Sols { get; set; }
        public List<int> Edges { get; set; }

        public Calendar(List<int> sols)
        {
            Values = new char[Height * Width];
            Array.Fill(Values, 'X');

            Sols = sols;

            Edges = new List<int>() { 6, 13, 49, 50, 51, 52 };
            foreach (int edge in Edges)
            {
                Values[edge] = 'E';
            }

            foreach (int sol in sols)
            {
                Values[sol] = 'S';
            }
        }

        public Calendar(List<int> sols, Calendar calendarToCopy) : this(sols)
        {
            Values = new char[Height * Width];
            Array.Fill(Values, 'X');

            for (int i = 0; i < calendarToCopy.Values.Length; i++)
            {
                Values[i] = calendarToCopy.Values[i];
            }
        }

        public bool LayShape(int[,] shape, char pRep)
        {
            var numColsPiece = shape.GetLength(1);
            var numRowsPiece = shape.GetLength(0);

            //grid space to be laid
            var firstAvailableSpace = Array.IndexOf(Values, 'X');

            //find the number of spaces to shift the lay left so that the grid
            //space gets filled with part of a piece instead of empty space
            var colShift = GetColShift(numColsPiece, shape);

            //Determine if there are any obstructions to the piece being laid
            //If no obstructions - copy over the contents of the piece shape 
            //to each index to be covered with it
            if (LayValid(firstAvailableSpace, colShift, shape))
            {
                for (int x = 0; x < numRowsPiece; ++x)
                {
                    for (int y = 0; y < numColsPiece; ++y)
                    {
                        int index = y + (firstAvailableSpace - colShift) + x * Width;

                        if (Values[index] == 'X' && shape[x, y] != 0)
                        {
                            Values[index] = pRep;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        private bool LayValid(int firstAvailableSpace, int colShift, int[,] shape)
        {
            var numColsPiece = shape.GetLength(1);//put in piece class
            var numRowsPiece = shape.GetLength(0);

            for (int x = 0; x < numRowsPiece; ++x)
            {
                for (int y = 0; y < numColsPiece; ++y)
                {
                    int index = y + (firstAvailableSpace - colShift) + x * Width;

                    //obstruction case: index is off the board
                    //obstruction case: index was shifted too far left for piece to fit
                    //obstruction case: index is an edge
                    //obstruction case: index is already filled with another piece
                    if (index > Values.Length - 1
                        || index < 0
                        || Edges.Contains(index)
                        || (index % Width == 0 && y != 0)
                        || Values[index] != 'X' && shape[x, y] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int GetColShift(int numColsPiece, int[,] shape)
        {
            var colShift = 0;
            for (int i = 0; i < numColsPiece; i++)
            {
                if (shape[0, i] != 0)
                {
                    break;
                }

                colShift++;
            }

            return colShift;
        }

        public void PrintCalendar()
        {
            var index = 0;
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Console.Write(string.Format("{0} ", Values[index]));
                    index++;
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            Console.Write("----------------" + Environment.NewLine + Environment.NewLine);
        }
    }
}