Rooms[,] RoomArea = new Rooms[7,7];
bool[,] visited = new bool[7,7];
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




Main(RoomArea, Start, Garden, Clearances, visited);
static void Main(Rooms[,] RoomArea, Rooms Start, Rooms Exit, Rooms[][]Clearances, bool[,] visited){

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
    visited[Start1, Start2] = true;
    RoomArea[Exit1, Exit2] = Exit;
    visited[Exit1, Exit2] = true;

    RoomArea = SpawnRoomsBetween(Exit1, Exit2, Start1, Start2, RoomArea, Clearances, visited);
    //RoomArea = SpawnRoomsBeside(RoomArea, Clearances, Start1, Start2, 0, visited);
     RoomArea = SetNullRoom(RoomArea, Clearances, visited);



     for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
        for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
            Console.Write($"{visited[Xcolumn, Ycolumn]} ");
        }
            Console.WriteLine();
    }
   

       Console.WriteLine("");

    for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
        for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
            Console.Write($"{RoomArea[Xcolumn, Ycolumn].RoomClearanceLvl} ");
        }
            Console.WriteLine();
    }
 
 Console.ReadKey();

}
    	//Spawns a path of rooms between Start and Exit
    static Rooms[,] SpawnRoomsBetween(int ExitX, int ExitY, int StartX, int StartY , Rooms[,] RoomArea, Rooms[][]Clearances, bool[,] visited){

       int currentX = StartX;
       int currentY = StartY;
       List<(int, int)> path = new List<(int, int)>();

        while(currentX != ExitX || currentY != ExitY){
            if(currentX != ExitX){
                currentX += MathF.Sign(ExitX - currentX);
                
            }
            if(currentY != ExitY){
                currentY += MathF.Sign(ExitX - currentY);
            }
            path.Add((currentX,currentY));
        }
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




    static Rooms[,] SpawnRoomsBeside(Rooms[,] RoomArea, Rooms[][] Clearances, int Xpos, int Ypos, int Clearance, bool[,] visited){

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
static Rooms[,] SetNullRoom(Rooms[,] RoomArea, Rooms[][] Clearances, bool[,] visited){

    for(int Ycolumn = 0; Ycolumn<RoomArea.GetLength(1); Ycolumn++){
        for(int Xcolumn = 0; Xcolumn < RoomArea.GetLength(0); Xcolumn++){
            if(RoomArea[Xcolumn,Ycolumn] == null){
                RoomArea[Xcolumn, Ycolumn] = Clearances[0][0];
            }
        }
    }

return RoomArea;

}







