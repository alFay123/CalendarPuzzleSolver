using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarPuzzleSolver
{
    public class Piece
    {
        public char Rep { get; set; }
        public List<int[,]> Shapes { get; set; }

        public Piece(int[,] originalShape, char rep)
        {
            Shapes = getPossibleShapes(originalShape);
            Rep = rep;
        }

        //Find all possible shapes of the piece: rotations and front-to-back flips
        private List<int[,]> getPossibleShapes(int[,] piece)
        {
            var possibleShapes = new List<int[,]>();

            //add the original shape and its flip
            possibleShapes.Add(piece);
            possibleShapes.Add(getFlip(piece));

            //Pieces can be rotated 90 degrees 3 times
            for (int i = 0; i < 3; i++)
            {
                //add the rotation if unique
                var rotation = getRotation(piece);
                if (IsUnique(rotation, possibleShapes))
                {
                    possibleShapes.Add(rotation);
                }

                //now add the flip of the rotation if unique
                var flip = getFlip(rotation);
                if (IsUnique(flip, possibleShapes))
                {
                    possibleShapes.Add(flip);
                }

                piece = rotation;
            }

            return possibleShapes;
        }

        private static int[,] getRotation(int[,] shape)
        {
            var rowLen = shape.GetLength(0);
            var colLen = shape.GetLength(1);

            var rotation = new int[colLen, rowLen];
            for (int x = 0; x < rowLen; x++)
            {
                for (int y = 0; y < colLen; y++)
                {
                    rotation[y, rowLen - x - 1] = shape[x, y];
                }
            }

            return rotation;
        }

        public static int[,] getFlip(int[,] shape)
        {
            var rowLen = shape.GetLength(0);
            var colLen = shape.GetLength(1);

            var flip = new int[rowLen, colLen];
            for (int x = 0; x < rowLen; x++)
            {
                for (int y = 0; y < colLen; y++)
                {
                    flip[x, colLen - y - 1] = shape[x, y];
                }
            }

            return flip;
        }

        private bool IsUnique(int[,] newPieceLayout, List<int[,]> pieceLayouts)
        {
            foreach (var pieceLayout in pieceLayouts)
            {
                if (IsEqual(pieceLayout, newPieceLayout))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsEqual(int[,] a, int[,] b)
        {
            if (a.GetLength(0) == b.GetLength(0)
                && a.GetLength(1) == b.GetLength(1))
            {
                for (int x = 0; x < a.GetLength(0); x++)
                {
                    for (int y = 0; y < a.GetLength(1); y++)
                    {
                        if (a[x, y] != b[x, y])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}
