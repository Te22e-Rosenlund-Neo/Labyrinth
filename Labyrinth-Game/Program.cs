using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
Rooms[,] RoomArea = new Rooms[7,7];
Random Rgen = new();

Rooms Start = new(0);    
Rooms Corridor = new(0); 
Rooms[] Clearance0 = { Start ,Corridor};

Rooms Kitchen = new(1); 
Rooms Bedroom = new(1); 
Rooms BathingHall = new(1); 
Rooms Library = new(1); 
Rooms[] Clearance1 = {Kitchen, Bedroom, BathingHall, Library};

Rooms TortureChamber = new(2); 
Rooms MainHall = new(2); 
Rooms BathRoom = new(2); 
Rooms[] Clearance2 = {TortureChamber, MainHall, BathRoom};

Rooms Garden = new(3); // 1 openings EXIT
Rooms[] Final = {Garden};

Rooms[][] Clearances = {Clearance0, Clearance1, Clearance2, Final};
bool[,] Visited = new bool[15, 15];



static void Main(Rooms[,] RoomArea, Rooms Start, Rooms Exit, Rooms[][]Clearances){

    Items KeyOne = new Items(1);
    Items KeyTwo = new Items(2);
    Items ExitKey = new Items(3);

    int Start1 = Random.Shared.Next(0, RoomArea.GetLength(0));
    int Start2 = Random.Shared.Next(0, RoomArea.GetLength(1));
    int Exit1 = Start1;
    int Exit2 = Start2;
    while (Exit1 == Start1 && Exit2 == Start2){
         Exit1 = Random.Shared.Next(0, RoomArea.GetLength(0));
         Exit2 = Random.Shared.Next(0, RoomArea.GetLength(1));
    }

    RoomArea[Start1, Start2] = Start;
    RoomArea[Exit1, Exit2] = Exit;
    //path calculates how we way from start to exit, 
    int PathX = Exit1 - Start1;
    int PathY = Exit2 - Start2;
    RoomArea = SpawnRoomsBetween(PathX, PathY, Start1, Start2, RoomArea, Clearances);
    RoomArea = SpawnRoomsBeside(RoomArea, Clearances, Start1, Start2, 0);

///FOR ROOM CLEARANCE  = 0 IT WONT WORK!!!!!!!!



}
    	//Spawns a path of rooms between Start and Exit
    static Rooms[,] SpawnRoomsBetween(int PathX, int PathY, int Startpos1, int Startpos2 , Rooms[,] RoomArea, Rooms[][]Clearances){

        for(int i = Startpos1; i == PathX; i += 1 * MathF.Sign(PathX)){
            if(i == Startpos1 +- 1){
                RoomArea[Startpos1 + i, Startpos2] = Clearances[1][Random.Shared.Next(0, Clearances[1].Length)];
            }else{
                RoomArea[Startpos1 + i, Startpos2] = Clearances[2][Random.Shared.Next(0, Clearances[2].Length)];
            }            
        }
        for(int j = Startpos2; j == PathY; j += 1* MathF.Sign(PathY)){
            RoomArea[PathX, Startpos2 + j] = Clearances[2][Random.Shared.Next(0, Clearances[2].Length)];
        }
         return RoomArea;
    }

    static Rooms[,] SpawnRoomsBeside(Rooms[,] RoomArea, Rooms[][] Clearances, int Xpos, int Ypos, int Clearance){

        if(RoomArea[Xpos+1, Ypos] != null && RoomsExist(Xpos + 1, Ypos, RoomArea)){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,1) * 2 - 1; 
            int NewClearance = Clearance + ClearanceNext;
            RoomArea[Xpos+1, Ypos] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos+1, Ypos, NewClearance);
        }
       if(RoomArea[Xpos-1, Ypos] != null && RoomsExist(Xpos-1, Ypos, RoomArea)){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,1) * 2 - 1;
            int NewClearance = Clearance + ClearanceNext;
            RoomArea[Xpos-1, Ypos] = Clearances[ClearanceNext][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos+1, Ypos, NewClearance); 
        } 
       if(RoomArea[Xpos, Ypos+1] != null && RoomsExist(Xpos, Ypos+1, RoomArea)){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,1) * 2 - 1;
            int NewClearance = Clearance + ClearanceNext;
            RoomArea[Xpos, Ypos+1] = Clearances[ClearanceNext][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos, Ypos+1, NewClearance); 
        } 
       if(RoomArea[Xpos, Ypos-1] != null && RoomsExist(Xpos, Ypos-1, RoomArea)){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,1) * 2 - 1;
            int NewClearance = Clearance + ClearanceNext;
            RoomArea[Xpos, Ypos-1] = Clearances[ClearanceNext][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos, Ypos-1, NewClearance); 
        } 
        
        return RoomArea;



    }






//tests if the designated room is possible inside of the array (AKA not ArrayOutOfBoundsException)

static bool RoomsExist(int x, int y, Rooms[,] RoomArea){
if(x > RoomArea.GetLength(0) || x < 0 || y > RoomArea.GetLength(1) || y < 0){
    return false;
}else{
    return true;
}
}





Console.ReadKey();

