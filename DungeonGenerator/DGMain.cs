using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    public class DGMain
    {
        private static SettingsGenerator SG = SGFill();
        private static SettingsLayout SL;

        private static char[,] Dungeon; //2D Dungeon

        #region Random_and_Seed
        private static Random dR = new Random();
        private static int seed = dR.Next();
        private static Random R = new Random(seed);
        #endregion

        public DGMain(SettingsLayout SL_get)
        {
            SL = SL_get; //Makes it global
            Dungeon = new char[SL.MapWidth, SL.MapHeight];

            EnforceANDCheckValues(); //Enforces correct input data
            StartGenerate(); //Generates the Dungeon 

        }

        


        static void StartGenerate()
        {
            for (int i = 0; i < Dungeon.GetLength(0); i++)
            {
                Dungeon[i, 0] = SG.Rooms[0];
                Dungeon[i, Dungeon.GetLength(1) - 1] = SG.Rooms[0];
            } //Creates the border in one axis
            for (int i = 0; i < Dungeon.GetLength(1); i++)
            {
                Dungeon[0, i] = SG.Rooms[0];
                Dungeon[Dungeon.GetLength(0) - 1, i] = SG.Rooms[0];
            } //Creates the border in the other axis

            VoidGenerate();

            int startX, startY, notFoundCounter = 0;
            do
            {
                notFoundCounter++;
                startX = R.Next(2, SL.MapWidth - 2);
                startY = R.Next(2, SL.MapHeight - 2);
                //startX = new Random(seed).Next(2, SL.MapWidth - 2);
                //startY = new Random(seed).Next(2, SL.MapHeight - 2);
            } while (notFoundCounter < 300 && (
                     (Dungeon[startX, startY] != '\0' && Dungeon[startX - 1, startY] != '\0') && 
                     (Dungeon[startX, startY] != '\0' && Dungeon[startX + 1, startY] != '\0') && 
                     (Dungeon[startX, startY] != '\0' && Dungeon[startX, startY - 1] != '\0') && 
                     (Dungeon[startX, startY] != '\0' && Dungeon[startX, startY + 1] != '\0'))); //Makes sure that there are at least 2 tiles to start with and also limits the number of attempts

            Dungeon[startX, startY] = '\0';
            Dungeon[startX - 1, startY] = '\0';
            Dungeon[startX + 1, startY] = '\0';
            Dungeon[startX, startY - 1] = '\0';
            Dungeon[startX, startY + 1] = '\0';
            Dungeon[startX - 1, startY - 1] = '\0';
            Dungeon[startX + 1, startY + 1] = '\0';
            Dungeon[startX + 1, startY - 1] = '\0';
            Dungeon[startX - 1, startY + 1] = '\0';

            //if (notFoundCounter > 299)
            {
                Dungeon[startX, startY] = '\0';
                Dungeon[startX - 1, startY] = '\0';
                Dungeon[startX + 1, startY] = '\0';
                Dungeon[startX, startY - 1] = '\0';
                Dungeon[startX, startY + 1] = '\0';
                Dungeon[startX - 1, startY - 1] = '\0';
                Dungeon[startX + 1, startY + 1] = '\0';
                Dungeon[startX + 1, startY - 1] = '\0';
                Dungeon[startX - 1, startY + 1] = '\0';
            } //Creates a 3x3 playable area if none was found

            Dungeon[startX, startY] = PickRoom(new List<int> { 1, 2, 3, 4, 5/*, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15*/ });
            //Dungeon[startX, startY] = PickRoom(new List<int> { 11 });

            BacktrackGenerate(startX, startY);
        } 

        static void BacktrackGenerate(int CurrentX, int CurrentY)
        {
            //Dungeon[CurrentX, CurrentY] = 'X';

            if (Dungeon[CurrentX, CurrentY] == SG.Rooms[1]) //┼ 
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(1, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }

                if (Dungeon[CurrentX, CurrentY + 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(1, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }

                if (Dungeon[CurrentX + 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(1, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }

                if (Dungeon[CurrentX, CurrentY - 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(1, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[2]) //┴
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(2, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }

                if (Dungeon[CurrentX, CurrentY + 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(2, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }

                if (Dungeon[CurrentX, CurrentY - 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(2, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[3]) //┬
            {
                if (Dungeon[CurrentX, CurrentY + 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(3, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }
                

                if (Dungeon[CurrentX + 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(3, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }
                

                if (Dungeon[CurrentX, CurrentY - 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(3, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[4]) //┤
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(4, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }

                if (Dungeon[CurrentX + 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(4, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }

                if (Dungeon[CurrentX, CurrentY - 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(4, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[5]) //├
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(5, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }

                if (Dungeon[CurrentX, CurrentY + 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(5, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }

                if (Dungeon[CurrentX + 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(5, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[6]) //─
            {
                if (Dungeon[CurrentX, CurrentY + 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(6, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }

                if (Dungeon[CurrentX, CurrentY - 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(6, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[7]) //│
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(7, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }

                if (Dungeon[CurrentX + 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(7, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[8]) //┌
            {
                if (Dungeon[CurrentX, CurrentY + 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(8, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }

                if (Dungeon[CurrentX + 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(8, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[9]) //┐
            {
                if (Dungeon[CurrentX + 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(9, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }

                if (Dungeon[CurrentX, CurrentY - 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(9, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[10]) //└
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(10, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }
                
                if (Dungeon[CurrentX, CurrentY + 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(10, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[11]) //┘
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0')
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(11, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }
                
                if (Dungeon[CurrentX, CurrentY - 1] == '\0')
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(11, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[12]) //╵
            {
                if (Dungeon[CurrentX - 1, CurrentY] == '\0' /*|| Dungeon[CurrentX - 1, CurrentY] == SG.Rooms[0]*/)
                {
                    Dungeon[CurrentX - 1, CurrentY] = PickRoom(possibleListCreator(12, 0, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX - 1, CurrentY);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[13]) //╶
            {
                if (Dungeon[CurrentX, CurrentY + 1] == '\0' /*|| Dungeon[CurrentX, CurrentY + 1] == SG.Rooms[0]*/)
                {
                    Dungeon[CurrentX, CurrentY + 1] = PickRoom(possibleListCreator(13, 1, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY + 1);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[14]) //╷
            {
                if (Dungeon[CurrentX + 1, CurrentY] == '\0' /*|| Dungeon[CurrentX + 1, CurrentY] == SG.Rooms[0]*/)
                {
                    Dungeon[CurrentX + 1, CurrentY] = PickRoom(possibleListCreator(14, 2, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX + 1, CurrentY);
                }
                
            }
            else if (Dungeon[CurrentX, CurrentY] == SG.Rooms[15]) //╴
            {
                if (Dungeon[CurrentX, CurrentY - 1] == '\0' /*|| Dungeon[CurrentX, CurrentY - 1] == SG.Rooms[0]*/)
                {
                    Dungeon[CurrentX, CurrentY - 1] = PickRoom(possibleListCreator(15, 3, CurrentX, CurrentY));
                    BacktrackGenerate(CurrentX, CurrentY - 1);
                }
                
            }
            else
            {
                //??? //Dungeon[CurrentX, CurrentY] == 0 or X>15
            }

        }

        static void VoidGenerate()
        {
            #region values
            //if (SL.Noise > 70) { SL.Noise = 70; } //Enforce upper limit
            //if (SL.Noise < 0) { SL.Noise = 0; } //Enforce lower limit

            //if (SL.NoiseClusterSize > 3) { SL.NoiseClusterSize = 3; } //Enforce upper limit
            //if (SL.NoiseClusterSize < 0) { SL.NoiseClusterSize = 0; } //Enforce lower limit

            int VoidNeeded;
            List<int> locationX = new List<int>();
            List<int> locationY = new List<int>();


            if (Convert.ToString(SL.Noise).Length == 1) { VoidNeeded = Convert.ToInt32(Dungeon.Length * Convert.ToDouble("0.0" + Convert.ToString(SL.Noise))); }
            else { VoidNeeded = Convert.ToInt32(Dungeon.Length * Convert.ToDouble("0." + Convert.ToString(SL.Noise))); }

            VoidNeeded = Convert.ToInt32(VoidNeeded / Math.Pow(SL.NoiseClusterSize, 2));
            #endregion

            for (int i = 0; i < VoidNeeded; i++)
            {
                locationX.Add(R.Next(Dungeon.GetLength(0)));
                locationY.Add(R.Next(Dungeon.GetLength(1)));

                Dungeon[locationX[i], locationY[i]] = SG.Rooms[0];
            } //Noise Location Determiner


            int r = Convert.ToInt32(Math.Pow(SL.NoiseClusterSize, 2));
            //NOTE: (x-a)^2 + (y-b)^2 + 0.49 <= r^2

            char[,] cluster = new char[SL.NoiseClusterSize * 2 + 1, SL.NoiseClusterSize * 2 + 1];
            for (int x = 0; x < cluster.GetLength(0); x++)
            {
                for (int y = 0; y < cluster.GetLength(1); y++)
                {
                    if (Math.Pow(x - SL.NoiseClusterSize, 2) + Math.Pow(y - SL.NoiseClusterSize, 2) + 0.49 <= r)
                    {
                        cluster[x, y] = SG.Rooms[0];
                    }
                }
            } //NoiseClusterSize Generator

            for (int i = 0; i < VoidNeeded; i++)
            {
                for (int x = locationX[i] - SL.NoiseClusterSize; x - locationX[i] < cluster.GetLength(0); x++)
                {
                    for (int y = locationY[i] - SL.NoiseClusterSize; y - locationY[i] < cluster.GetLength(1); y++)
                    {
                        try
                        {
                            if (cluster[x - locationX[i], y - locationY[i]] == SG.Rooms[0])
                            {
                                Dungeon[x - SL.NoiseClusterSize, y - SL.NoiseClusterSize] = cluster[x - locationX[i], y - locationY[i]];
                            }
                        }
                        catch (Exception)
                        {
                            //Console.WriteLine("Outside of array:" + x + ";" + y);
                        }
                    }
                }


                //TODO: Randomize cluster shape 


            }
        }

        static List<int> possibleListCreator(int current, int direction, int X, int Y)
        {
            if (direction == 0) { X--; }
            else if (direction == 1) { Y++; }
            else if (direction == 2) { X++; }
            else { Y--; }

            List<int> AllowedNeighboursN = new List<int>();
            List<int> AllowedNeighboursE = new List<int>();
            List<int> AllowedNeighboursS = new List<int>();
            List<int> AllowedNeighboursW = new List<int>();

            int Ni = WhereIsindex(Dungeon[X-1,Y]);
            int Ei = WhereIsindex(Dungeon[X,Y+1]);
            int Si = WhereIsindex(Dungeon[X+1,Y]);
            int Wi = WhereIsindex(Dungeon[X,Y-1]);

            if (Ni != -1)
            {
                AllowedNeighboursN = SG.AllowedNeighbours[Ni][2];
            }
            else
            {
                for (int i = 0; i < SG.Rooms.Count(); i++)
                {
                    AllowedNeighboursN.Add(i);
                }
            } //Gets all possible rooms depending on the northern room

            if (Ei != -1)
            {
                AllowedNeighboursE = SG.AllowedNeighbours[Ei][3];
            }
            else
            {
                for (int i = 0; i < SG.Rooms.Count; i++)
                {
                    AllowedNeighboursE.Add(i);
                }
            } //Gets all possible rooms depending on the eastern room

            if (Si != -1)
            {
                AllowedNeighboursS = SG.AllowedNeighbours[Si][0];
            }
            else
            {
                for (int i = 0; i < SG.Rooms.Count; i++)
                {
                    AllowedNeighboursS.Add(i);
                }
            } //Gets all possible rooms depending on the southern room

            if (Wi != -1)
            {
                AllowedNeighboursW = SG.AllowedNeighbours[Wi][1];
            }
            else
            {
                for (int i = 0; i < SG.Rooms.Count; i++)
                {
                    AllowedNeighboursW.Add(i);
                }
            } //Gets all possible rooms depending on the western room

            List<int> NS = AllowedNeighboursN.Intersect(AllowedNeighboursS).ToList();
            List<int> EW = AllowedNeighboursE.Intersect(AllowedNeighboursW).ToList();
            List<int> NESW = NS.Intersect(EW).ToList();

            return NESW.Intersect(SG.AllowedNeighboursDirect[current][direction]).ToList();

            //return SG.AllowedNeighboursDirect[current][direction]; //Not enough >> needs to "look around" for bordering tile contents
        }

        static char PickRoom(List<int> possibleList)
        {
            if (possibleList.Count > 0)
            {
                try{ return SG.Rooms[possibleList[R.Next(possibleList.Count())]]; }
                catch (Exception) { return 'X'; }
                //try { return SG.Rooms[possibleList[new Random(seed).Next(possibleList.Count())]]; }
                //catch (Exception) { return 'X'; }
            }
            return '\0';
            //return SA.SPG.Rooms[possible[R.Next(possible.Count())]];
        } //Selects a random possible room from the given list

        static int WhereIsindex(char questioned)
        {
            for (int i = 0; i < SG.Rooms.Count(); i++) { if (questioned == SG.Rooms[i]) { return i; } }
            return -1;
        } //Locates the questioned char-s index

        static void EnforceANDCheckValues()
        {
            //if (SL.Seed != 0) { seed = SL.Seed; } //Checks if the user provided a seed or not.


            if (SL.MapHeight < 7) { SL.MapHeight = 7; } //Enforce minimal map height
            if (SL.MapWidth < 7) { SL.MapWidth = 7; } //Enforce minimal map width

            if (SL.MapHeight > 52) { SL.MapHeight = 52; } //Enforce maximal map height
            if (SL.MapWidth > 52) { SL.MapWidth = 52; } //Enforce maximal map width


            if (SL.Noise > 30) { SL.Noise = 30; } //Enforce upper limit
            if (SL.Noise < 0) { SL.Noise = 0; } //Enforce lower limit

            if (SL.NoiseClusterSize > 5) { SL.NoiseClusterSize = 5; } //Enforce upper limit
            if (SL.NoiseClusterSize < 1) { SL.NoiseClusterSize = 1; } //Enforce lower limit
        }

        #region Structs
        public struct SettingsGenerator
        {
            public List<char> Rooms;
            //The char-s that represents a room/space in the 2D matrix 
            public List<List<int>[]> AllowedNeighbours; //Perfect for headaches. ~Love, your past self :3
            //The char-s that can be next to the given char 
            //List >> Array >> List
            //The main List contains Arrays that each have 4 Lists
            //Array >>> UP - RIGHT - DOWN - LEFT in order
            public List<List<int>[]> AllowedNeighboursDirect;
        }

        public struct SettingsLayout
        {
            public int MapWidth; //Width of the map WITH BORDER
            public int MapHeight; //Height of the map WITH BORDER
            public int Seed; //Seed for >>> static Random R = new Random(int Seed);
            public int Noise; //Pre-populated blank spaces in layout in %. If Noise > 70 >>> Noise = 70
            public int NoiseClusterSize; //Size of the blank spaces in layout
        }

        static SettingsGenerator SGFill()
        {
            SettingsGenerator SGtemp = new SettingsGenerator();

            SGtemp.Rooms = new List<char> { ' ', '┼', '┴', '┬', '┤', '├', '─', '│', '┌', '┐', '└', '┘', '╵', '╶', '╷', '╴' };

            SGtemp.AllowedNeighbours = new List<List<int>[]> { new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 } ,
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15  },
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                           new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13/*15*/ } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 },
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                           new List<int>[] {
                                                                new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 },
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 },
                                                                new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                new List<int> { 0, 4, 7, 9, 11, 12, 14, 15 } },
                                                           new List<int>[] {
                                                                new List<int> { 0, 2, 6, 10, 11, 12, 13, 15 },
                                                                new List<int> { 0, 5, 7, 8, 10, 12, 13, 14 },
                                                                new List<int> { 0, 3, 6, 8, 9, 13, 14, 15 },
                                                                new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } }, };

            SGtemp.AllowedNeighboursDirect = new List<List<int>[]> { new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { } ,
                                                                      new List<int> { },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11, 15  },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                                 new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11, 12 },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11, 15 },
                                                                      new List<int> { },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9, 14 },
                                                                      new List<int> { },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10, 13 } },
                                                                 new List<int>[] {
                                                                      new List<int> { 1, 3, 4, 5, 7, 8, 9/*, 14*/ },
                                                                      new List<int> { },
                                                                      new List<int> { },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 4, 6, 9, 11/*, 15*/ },
                                                                      new List<int> { },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 4, 5, 7, 10, 11/*, 12*/ },
                                                                      new List<int> { } },
                                                                 new List<int>[] {
                                                                      new List<int> { },
                                                                      new List<int> { },
                                                                      new List<int> { },
                                                                      new List<int> { 1, 2, 3, 5, 6, 8, 10/*, 13*/ } }, };

            return SGtemp;
        }
        #endregion

        public char[,] TheDungeon
        {
            get { return Dungeon; }
        } //To access the finished Dungeon outside of this class
    }
}
