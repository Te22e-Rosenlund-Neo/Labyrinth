using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
Rooms[,] RoomArea = new Rooms[7,7];
bool[,] Visited = new bool[7,7];
Random Rgen = new();

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

Rooms Garden = new(3,"Garden"); // 1 openings EXIT
Rooms[] Final = {Garden};

Rooms[][] Clearances = {Clearance0, Clearance1, Clearance2};

Main(RoomArea, Start, Garden, Clearances);

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
    
    //path calculates the  way from start to exit, 
    int PathX = Exit1 - Start1;
    int PathY = Exit2 - Start2;
    RoomArea = SpawnRoomsBetween(PathX, PathY, Start1, Start2, RoomArea, Clearances);
    RoomArea = SpawnRoomsBeside(RoomArea, Clearances, Start1, Start2, 0);
    RoomArea = SetNullRoom(RoomArea, Clearances);
    for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
        for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
            Console.Write($"{RoomArea[Xcolumn, Ycolumn].RoomClearanceLvl} ");
        }
            Console.WriteLine();
    }
    Console.ReadKey();



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

            if(RoomsExist(Xpos + 1, Ypos, RoomArea)){
        if(RoomArea[Xpos+1, Ypos] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1; 
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos+1, Ypos] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos+1, Ypos, NewClearance);
        }}
        if(RoomsExist(Xpos-1, Ypos, RoomArea)){
       if(RoomArea[Xpos-1, Ypos] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1;
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos-1, Ypos] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos+1, Ypos, NewClearance); 
        } }
        if(RoomsExist(Xpos, Ypos+1, RoomArea)){
       if(RoomArea[Xpos, Ypos+1] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1;
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos, Ypos+1] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos, Ypos+1, NewClearance); 
        } }
        if(RoomsExist(Xpos, Ypos-1, RoomArea)){
       if(RoomArea[Xpos, Ypos-1] == null){
            //returns either 1 or -1
            int ClearanceNext = Random.Shared.Next(0,2) * 2 - 1;
            int NewClearance = ClearanceCheck(Clearance + ClearanceNext, Clearances);
            RoomArea[Xpos, Ypos-1] = Clearances[NewClearance][Random.Shared.Next(0, Clearances[NewClearance].Length)];
            SpawnRoomsBeside(RoomArea, Clearances, Xpos, Ypos-1, NewClearance); 
        } 
        }
        return RoomArea;
    }
    //}






//tests if the designated room is possible inside of the array (AKA not ArrayOutOfBoundsException)

static bool RoomsExist(int x, int y, Rooms[,] RoomArea){
if(x >= 0 && x < RoomArea.GetLength(0) && y >= 0 && y < RoomArea.GetLength(1)){
    return true;
}else{
    return false;
}
}
static int ClearanceCheck(int clearancelevel, Rooms[][] Clearances){

    if(clearancelevel > Clearances.Length-1){
        return clearancelevel - 2;

    }else if(clearancelevel < 0){
        return clearancelevel + 1;

    }else{
        return clearancelevel;
    }
}
static Rooms[,] SetNullRoom(Rooms[,] RoomArea, Rooms[][] Clearances){

    for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
        for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
            if(RoomArea[Xcolumn,Ycolumn] == null){
                SpawnRoomsBeside(RoomArea, Clearances, Xcolumn, Ycolumn, Random.Shared.Next(0,Clearances.Length));
            }
        }
    }

return RoomArea;

}







