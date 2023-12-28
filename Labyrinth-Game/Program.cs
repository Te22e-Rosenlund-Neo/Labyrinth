Console.OutputEncoding = System.Text.Encoding.UTF8;
//RoomArea is an array which houses our playing field, visisted is for bug/generation bug fixes
Rooms[,] RoomArea = new Rooms[7,7];
bool[,] visited = new bool[7,7];
bool[,] CurrentMoveHolder = new bool[7,7];

//here we declare a bunch of objects of type rooms that we later randomize from
//first parameter is the clearance level, which the player needs a key corresponding to the same clearance level to access
Rooms Start = new(-1, "Start");    
Rooms Corridor = new(0,"Corridor"); 
Rooms[] Clearance0 = {Corridor};

Rooms Kitchen = new(1,"Kitchen"); 
Rooms Bedroom = new(1,"Bedroom"); 
Rooms BathingHall = new(1,"BathingHall"); 
Rooms Library = new(1,"Library"); 
Rooms[] Clearance1 = {Kitchen, Bedroom, BathingHall, Library};

Rooms TortureChamber = new(2,"TortureChamber"); 
Rooms MainHall = new(2,"MainHall"); 
Rooms BathRoom = new(2,"BathRoom"); 
Rooms[] Clearance2 = {TortureChamber, MainHall, BathRoom};

Rooms Garden = new(3,"Garden"); 
//this array lets us randomly pick one of the 3 types of rooms to randomly generate
Rooms[][] Clearances = {Clearance0, Clearance1, Clearance2};

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//our main method, from here all methods are called
Main(RoomArea, Start, Garden, Clearances, visited);
static void Main(Rooms[,] RoomArea, Rooms Start, Rooms Exit, Rooms[][]Clearances, bool[,] visited){

    //key corresponds to what rooms player can enter
    Items KeyOne = new Items(1);
    Items KeyTwo = new Items(2);
    Items ExitKey = new Items(3);

    var Create = RoomCreater(Start, Exit, RoomArea, Clearances, visited);
    RoomArea = Create.Item1;
    int Start1 = Create.Item2;
    int Start2 = Create.Item3;

    DisplayRooms(RoomArea, Start1, Start2);













//     //writes a output that shows if all positions in roomarea is visited
//      for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
//         for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
//             Console.Write($"{visited[Xcolumn, Ycolumn]} ");
//         }
//             Console.WriteLine();
//     }
   

//        Console.WriteLine("");

// //prints out all rooms for dev to see
//     for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
//         for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
//             Console.Write($"{RoomArea[Xcolumn, Ycolumn].RoomClearanceLvl} ");
//         }
//             Console.WriteLine();
//     }
 
 Console.ReadKey();

}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static (Rooms[,], int, int) RoomCreater(Rooms Start, Rooms Exit, Rooms[,] RoomArea, Rooms[][] Clearances, bool[,] visited){
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
    
    var R = (RoomArea, Start1, Start2);
    return R;
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
            

            if(RoomArea[x,y] == null){
                if(MathF.Abs(x - StartX) == 1 || MathF.Abs(y - StartY) == 1 && DoOnce == true){
                    
                    RoomArea[x,y] = RoomArea[x, y] = Clearances[1][Random.Shared.Next(0, Clearances[1].Length)];
                    visited[x, y] = true;
                    DoOnce = false;          
            } else {
                RoomArea[x, y] = Clearances[2][Random.Shared.Next(0, Clearances[2].Length)];
                 visited[x, y] = true;
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
//checks if designated clearance level exists inside clearances array.
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

try{
    Console.WriteLine("                 " + RoomArea[p1-1,p2].RoomsName);
    Console.Write(RoomArea[p1,p2-1].RoomsName);
    Console.Write("     ⚫     ");
    Console.WriteLine(RoomArea[p1,p2+1].RoomsName);
    Console.WriteLine("                 " + RoomArea[p1+1,p2].RoomsName);
}catch(IndexOutOfRangeException){
    //does nothing, that exact statement will be skipped.
}
}

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static (int, int) MoveCheck(){

    string move = InputCheck();

    if(move == "W"){
        var Movement = (-1, 0);
        return Movement;

    }else if(move == "A"){
        var Movement = (0, -1);
        return Movement;

    }else if(move == "S"){
        var Movement = (1, 0);
        return Movement;

    }else if(move == "D"){
        var Movement = (0, 1);
        return Movement;

    }else{
        return (0,0);
    }

}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
static string InputCheck(){
    string[] InputType = {"W","A","S","D"};
    string Input = "";

    do{
        Console.WriteLine("Press a key to move (W, A, S, D)");
        Input = Console.ReadLine()!.ToUpper();

    } while(!InputType.Contains(Input));

return Input;
}



