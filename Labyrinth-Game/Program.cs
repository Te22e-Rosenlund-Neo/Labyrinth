using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
//RoomArea is an array which houses our playing field, visisted is for bug/generation bug fixes
Rooms[,] RoomArea = new Rooms[7,7];
bool[,] visited = new bool[7,7];
Items[,] KeyLocations = new Items[7,7];

//here we declare a bunch of objects of type rooms that we later randomize from
//first parameter is the clearance level, which the player needs a key corresponding to the same clearance level to access
Rooms Start = new(-1, "Start");    
Rooms Corridor = new(0,"Corridor(0)"); 
Rooms[] Clearance0 = {Corridor};

Rooms Kitchen = new(1,"Kitchen(1)"); 
Rooms Bedroom = new(1,"Bedroom(1)"); 
Rooms BathingHall = new(1,"BathingHall(1)"); 
Rooms Library = new(1,"Library(1)"); 
Rooms[] Clearance1 = {Kitchen, Bedroom, BathingHall, Library};

Rooms TortureChamber = new(2,"TortureChamber(2)"); 
Rooms MainHall = new(2,"MainHall(2)"); 
Rooms BathRoom = new(2,"BathRoom(2)"); 
Rooms[] Clearance2 = {TortureChamber, MainHall, BathRoom};

Rooms Garden = new(3,"Garden(3)"); 
//this array lets us randomly pick one of the 3 types of rooms to randomly generate
Rooms[][] Clearances = {Clearance0, Clearance1, Clearance2};

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//our main method, from here all methods are called
Main(RoomArea, Start, Garden, Clearances, visited, KeyLocations);
static void Main(Rooms[,] RoomArea, Rooms Start, Rooms Exit, Rooms[][]Clearances, bool[,] visited, Items[,] KeyLocations){

    //key corresponds to what rooms player can enter
    Items KeyOne = new Items(1);
    Items KeyTwo = new Items(2);
    Items ExitKey = new Items(3);



    //creates all rooms and sets players position to be the start room
    var Create = RoomCreater(Start, Exit, RoomArea, Clearances, visited);
    RoomArea = Create.Item1;
    int CurrentPos1 = Create.Item2;
    int CurrentPos2 = Create.Item3;

    //sets play loop, but you cant play infinite amount of times
    bool Play = true;
    int CurrentClearance = 0;

    //spawns all keys
    KeyLocations = SpawnKeys(KeyOne, KeyTwo, ExitKey, Create.Item2, Create.Item3, RoomArea, KeyLocations, Create.Item4, Create.Item5);
    
    while(Play == true){
        Console.Clear();
        DisplayRooms(RoomArea, CurrentPos1, CurrentPos2);

        //returns a direction that the player chose to move
        var Move = MoveCheck();
        int MoveX = CurrentPos1 + Move.Item1;
        int MoveY = CurrentPos2 + Move.Item2;
        //Checks if the room/direction that the player moved towards exists and that the player is allowed to enter it
        if(RoomsExist( MoveX , CurrentPos2, RoomArea) == true && RoomsExist(CurrentPos1, MoveY, RoomArea) == true){
            if(CurrentClearance >= RoomArea[MoveX, CurrentPos2].RoomClearanceLvl     &&  CurrentClearance >= RoomArea[CurrentPos1, MoveY].RoomClearanceLvl){
                CurrentPos1 += Move.Item1;
                CurrentPos2 += Move.Item2;
            }else{
                ChangeColour("You don't have the correct key to enter this room!", "Red");
            }
        }else{
            ChangeColour("You cannot move this way!", "Red");
        }
    
    //checks if where the player is, is at a loction with a key, if so increases players clearance level and removes the key
        if(KeyLocations[CurrentPos1, CurrentPos2] == KeyOne){
            CurrentClearance = 1;
            ChangeColour("You found Key 1!", "Green");
            KeyLocations[CurrentPos1, CurrentPos2] = null!;
        }else if(KeyLocations[CurrentPos1, CurrentPos2] == KeyTwo){
            CurrentClearance = 2;
            ChangeColour("You found Key 2!", "Green");
            KeyLocations[CurrentPos1, CurrentPos2] = null!;
        }else if(KeyLocations[CurrentPos1, CurrentPos2] == ExitKey){
            CurrentClearance = 3;
            ChangeColour("You found Key 3!", "Green");
            KeyLocations[CurrentPos1, CurrentPos2] = null!;
        }

    //if player entered the exit
        if(RoomArea[CurrentPos1, CurrentPos2] == Exit){
            ChangeColour("You Escaped!", "Green");
            break;
        }

        Thread.Sleep(2000);
    }






 //prints out all rooms for dev to see
    // for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
    //     for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
    //         Console.Write($"{RoomArea[Xcolumn, Ycolumn].RoomClearanceLvl} ");
    //     }
    //         Console.WriteLine();
    // }



//     //writes a output that shows if all positions in roomarea is visited
//      for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
//         for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
//             Console.Write($"{visited[Xcolumn, Ycolumn]} ");
//         }
//             Console.WriteLine();
//     }
   

//        Console.WriteLine("");


 
 Console.ReadKey();

}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static (Rooms[,], int, int, int, int) RoomCreater(Rooms Start, Rooms Exit, Rooms[,] RoomArea, Rooms[][] Clearances, bool[,] visited){
//creates start and exit rooms on random coordinates that can never be the same
    int Start1 = Random.Shared.Next(0, RoomArea.GetLength(0));
    int Start2 = Random.Shared.Next(0, RoomArea.GetLength(1));
    int Exit1 = Start1;
    int Exit2 = Start2;
    while (Exit1 == Start1 && Exit2 == Start2){
         Exit1 = Random.Shared.Next(0, RoomArea.GetLength(0));
         Exit2 = Random.Shared.Next(0, RoomArea.GetLength(1));
    }

    RoomArea[Start1, Start2] = Start;
    visited[Start1, Start2] = true;
    RoomArea[Exit1, Exit2] = Exit;
    visited[Exit1, Exit2] = true;

    // 2 method calls which generates all spots on our playing field 
    RoomArea = SpawnRoomsBetween(Exit1, Exit2, Start1, Start2, RoomArea, Clearances, visited);
    RoomArea = SpawnRoomsBeside(RoomArea, Clearances, Start1, Start2, 0, visited);
    
    var Retur = (RoomArea, Start1, Start2, Exit1, Exit2);
    return Retur;
}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    	//Spawns a path of rooms between Start and Exit
    static Rooms[,] SpawnRoomsBetween(int ExitX, int ExitY, int StartX, int StartY , Rooms[,] RoomArea, Rooms[][]Clearances, bool[,] visited){

       int currentX = StartX;
       int currentY = StartY;
    //this list holds a tuple for each position between start and exit in which needs a garuanteed clearance level so that the maze is always escapeable
    //I used a list instead of an array here because the size of our list will vary depending on the amount of positions between start and exit.
       List<(int, int)> path = new List<(int, int)>();

    
//here we add all the positions that needs a room for a garuanteed pathway
        while(currentX != ExitX){
            currentX += MathF.Sign(ExitX - currentX);
            path.Add((currentX, currentY));
        }
        while(currentY != ExitY){
            currentY += MathF.Sign(ExitY - currentY);
            path.Add((currentX,currentY));
        }
    //bool doOnce makes the first connected room be garuanteed a 1, after that they become 2's
        bool DoOnce = true;
        foreach(var position in path){
            int x = position.Item1;
            int y = position.Item2;
            
            //we then set the first two rooms to garuantee escapability, then the rest in the list/queue is random
            if(RoomArea[x,y] == null){
                if(MathF.Abs(x - StartX) == 1 || MathF.Abs(y - StartY) == 1 && DoOnce == true){
                    
                    RoomArea[x,y] = RoomArea[x, y] = Clearances[0][Random.Shared.Next(0, Clearances[0].Length)];
                    visited[x, y] = true;
                    DoOnce = false;          
            } else if(MathF.Abs(x - StartX) == 2 || MathF.Abs(y - StartY) == 2){
                RoomArea[x, y] = Clearances[1][Random.Shared.Next(0, Clearances[1].Length)];
                 visited[x, y] = true;
                }else if(MathF.Abs(x - StartX) > 2 || MathF.Abs(y - StartY) > 2){
                    int RandomClearance = Random.Shared.Next(0, Clearances.Length);
                    RoomArea[x, y] = Clearances[RandomClearance][Random.Shared.Next(0, Clearances[RandomClearance].Length)];
                }
            }
    }
    return RoomArea;

}


//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//spawns all the rest of the rooms in a procedural generation, where the rooms besides depends on a number of variables
    static Rooms[,] SpawnRoomsBeside(Rooms[,] RoomArea, Rooms[][] Clearances, int Xpos, int Ypos, int Clearance, bool[,] visited){

//these checks if the adjacent tile is existing, does not already have a value, and then generates a random room according to the clearances that surrounds it
//Clearancecheck makes sure that the next clearance for the room is possible. (not array out of bounds) 
            if(RoomsExist(Xpos + 1, Ypos, RoomArea)){
        if(RoomArea[Xpos+1, Ypos] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1; 
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos+1, Ypos] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            visited[Xpos+1, Ypos] = true;
            SpawnRoomsBeside(RoomArea, Clearances, Xpos+1, Ypos, NewClearance, visited);
        }}
        if(RoomsExist(Xpos-1, Ypos, RoomArea)){
       if(RoomArea[Xpos-1, Ypos] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1;
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos-1, Ypos] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
             visited[Xpos-1, Ypos] = true;
            SpawnRoomsBeside(RoomArea, Clearances, Xpos-1, Ypos, NewClearance, visited); 
        } }
        if(RoomsExist(Xpos, Ypos+1, RoomArea)){
       if(RoomArea[Xpos, Ypos+1] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1;
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos, Ypos+1] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
             visited[Xpos, Ypos+1] = true;
            SpawnRoomsBeside(RoomArea, Clearances, Xpos, Ypos+1, NewClearance, visited); 
        } }
        if(RoomsExist(Xpos, Ypos-1, RoomArea)){
       if(RoomArea[Xpos, Ypos-1] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1;
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos, Ypos-1] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
             visited[Xpos, Ypos-1] = true;
            SpawnRoomsBeside(RoomArea, Clearances, Xpos, Ypos-1, NewClearance, visited); 
        } 
        }
        return RoomArea;
    }
    //}





//-------------------------------------------------------------------------------------------------------------------------
//tests if the designated room is possible inside of the array (AKA not ArrayOutOfBoundsException)

static bool RoomsExist(int x, int y, Rooms[,] RoomArea){
if(x >= 0 && x < RoomArea.GetLength(0) && y >= 0 && y < RoomArea.GetLength(1)){
    return true;
}else{
    return false;
}
}
//-------------------------------------------------------------------------------------------------------------------------
//checks if designated clearance level exists inside clearances array(when generating maze).
static int ClearanceCheck(int clearancelevel, Rooms[][] Clearances){

    if(clearancelevel > Clearances.Length-1){
        return clearancelevel - 2;

    }else if(clearancelevel < 0){
        return clearancelevel + 1;

    }else{
        return clearancelevel;
    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static void DisplayRooms(Rooms[,] RoomArea, int p1, int p2){
    //here we first check if there is a room in all directions to display, if so we print out the rooms name. black dot represents our player
bool up, down, left, right;
if(RoomsExist(p1, p2-1, RoomArea) == true)  up = true; else{
    up = false;
}
if(RoomsExist(p1-1, p2, RoomArea) == true)  left = true;else{
    left = false;
}
if(RoomsExist(p1+1, p2, RoomArea) == true)  right = true;else{
    right = false;
}
if(RoomsExist(p1, p2+1, RoomArea) == true)  down = true;else{
    down = false;
}

if(up == true) Console.WriteLine("               " + RoomArea[p1,p2-1].RoomsName);
if(left == true) Console.Write(RoomArea[p1-1,p2].RoomsName);
Console.Write("     ⚫     ");
if(right == true) Console.Write(RoomArea[p1+1,p2].RoomsName);
if(down == true) Console.WriteLine("\n               " + RoomArea[p1,p2+1].RoomsName);


}

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------

static (int, int) MoveCheck(){

    //first we check if input is correct
    string move = InputCheck();
    //then depending on which key was pressed, we give a direction in a form of a tuple in which our positions will try to go towards
    if(move == "W"){
        var Movement = (0, -1);
        return Movement;

    }else if(move == "A"){
        var Movement = (-1, 0);
        return Movement;

    }else if(move == "S"){
        var Movement = (0, 1);
        return Movement;

    }else if(move == "D"){
        var Movement = (1, 0);
        return Movement;

    }else{
        return (0,0);
    }

}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static string InputCheck(){
    //we check if input is possible
    string[] InputType = {"W","A","S","D"};
    string Input = "";

//prompts the user at least once to enter a key, if wrong key we prompt again
    do{
        Console.WriteLine("");
        Console.WriteLine("Press a key to move (W, A, S, D)");
        Input = Console.ReadLine()!.ToUpper();

    } while(!InputType.Contains(Input));

return Input;
}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static void ChangeColour(string Sentence, string Colour){
    //simple function that changes texts color for better visualisation
if(Colour == "Red"){
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(Sentence);
    Console.ResetColor();
}else if(Colour == "Green"){
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(Sentence);
    Console.ResetColor();
}
}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static Items[,] SpawnKeys(Items KeyOne, Items KeyTwo, Items ExitKey, int Startpos1, int Startpos2, Rooms[,] RoomArea, Items[,] KeyLocations, int Exit1, int Exit2){
    //one list for each key, as we have to call the next keys FindPositions() using its values
List<(int, int)> Positions1 = new List<(int, int)>();
List<(int, int)> Positions2 = new List<(int, int)>();
List<(int, int)> Positions3 = new List<(int, int)>();

//finds locations that the first key can spawn in, and randomizes its location
bool[,] Visited = new bool[7,7];
Positions1 = FindPositions(Startpos1, Startpos2, 0, RoomArea, Visited, Positions1);                                                        //key 1
int RandLoc1 = Random.Shared.Next(0, Positions1.Count);
KeyLocations[Positions1[RandLoc1].Item1, Positions1[RandLoc1].Item2] = KeyOne;



//calls a new findpositions for each value in positions1, then randomises its position, in this case we are searching for 1's
bool[,] Visited1 = new bool[7,7];
foreach((int, int) pos in Positions1){
    Positions2 = FindPositions(pos.Item1, pos.Item2, 1, RoomArea, Visited1, Positions2);
}
int RandLoc2 = Random.Shared.Next(0, Positions2.Count);
KeyLocations[Positions2[RandLoc2].Item1, Positions2[RandLoc2].Item2] = KeyTwo;                //key 2



// //by the player simply having key one and key two, key 3 can therefor spawn completely ranom in the maze as the entire maze can now be explored (apart from the one exit room)
int RandlocX, RandlocY;
do{
 RandlocX = Random.Shared.Next(0, RoomArea.GetLength(0));
 RandlocY = Random.Shared.Next(0, RoomArea.GetLength(1));
 //does that while those positions is at the keyone or keytwo position or whilst it is at the exit rooms coordinates
}while((RandlocX == Positions1[RandLoc1].Item1 && RandlocY == Positions1[RandLoc1].Item2) || (RandlocX == Positions2[RandLoc2].Item1 && RandlocY == Positions1[RandLoc2].Item2) || (RandlocX == Exit1 && RandlocY == Exit2));
KeyLocations[RandlocX,RandlocY] = ExitKey;

return KeyLocations;
}


//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static List<(int, int)> FindPositions(int pos1, int pos2, int ClearanceToFind, Rooms[,] RoomArea, bool[,] visited, List<(int, int)> Position){

//when trying to place our key, we need to ensure that the key will always be accessible to the player, therefor this recursive function which works similar to BFS
//finds a the cluster of rooms with a designated clearancecode that is directly linked to the start or given position
//first we see if the rooms existsm then if it the correct clearance, then if it already has been visited, and then if that position already has been added
if(RoomsExist(pos1 + 1, pos2, RoomArea) == true){
    if(RoomArea[pos1 + 1, pos2].RoomClearanceLvl == ClearanceToFind){
        if(visited[pos1 + 1, pos2] == false){
            if(Position.Contains((pos1+1, pos2)) == false){
            Console.WriteLine($"{pos1+1},{pos2} was added");
            visited[pos1+1, pos2] = true;
            Position.Add((pos1 + 1, pos2));
            FindPositions(pos1+1, pos2, ClearanceToFind, RoomArea, visited, Position);
            }
        }
    }
}
if(RoomsExist(pos1 - 1, pos2, RoomArea) == true){
    if(RoomArea[pos1 - 1, pos2].RoomClearanceLvl == ClearanceToFind){
        if(visited[pos1 - 1, pos2] == false){
            if(Position.Contains((pos1-1, pos2)) == false){
            Console.WriteLine($"{pos1-1},{pos2} was added");
            visited[pos1-1, pos2] = true;
            Position.Add((pos1 - 1, pos2));
            FindPositions(pos1-1, pos2, ClearanceToFind, RoomArea, visited, Position);
            }
        }
    }
}
if(RoomsExist(pos1, pos2+1, RoomArea) == true){
    if(RoomArea[pos1, pos2+1].RoomClearanceLvl == ClearanceToFind){
        if(visited[pos1, pos2+1] == false){
            if(Position.Contains((pos1, pos2+1)) == false){
            Console.WriteLine($"{pos1},{pos2+1} was added");
            visited[pos1, pos2+1] = true;
            Position.Add((pos1, pos2+1));
            FindPositions(pos1, pos2+1, ClearanceToFind, RoomArea, visited, Position);
            }
        }
    }
}
if(RoomsExist(pos1, pos2-1, RoomArea) == true){
    if(RoomArea[pos1, pos2-1].RoomClearanceLvl == ClearanceToFind){
        if(visited[pos1, pos2-1] == false){
            if(Position.Contains((pos1, pos2-1)) == false){
            Console.WriteLine($"{pos1},{pos2-1} was added");
            visited[pos1, pos2-1] = true;
            Position.Add((pos1, pos2-1));
            FindPositions(pos1, pos2-1, ClearanceToFind, RoomArea, visited, Position);
            }
        }
    }
}
return Position;
}
