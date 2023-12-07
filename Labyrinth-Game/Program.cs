using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
Rooms[,] RoomArea = new Rooms[7,7];
Random Rgen = new();
Rooms Start = new(-1);    

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
    int Exit1 = Random.Shared.Next(0, RoomArea.GetLength(0));
    int Exit2 = Random.Shared.Next(0, RoomArea.GetLength(1));

    RoomArea[Start1, Start2] = Start;
    RoomArea[Exit1, Exit2] = Exit;
    int PathX = Exit1 - Start1;
    int PathY = Exit2 - Start2;

}

    void SpawnRoomsBetween(int PathX, int PathY, int Startpos1, int Startpos2 , Rooms[,] RoomArea, Rooms[][]Clearances){

   

       
         
                
            
                
            
        



    }





/*Rooms[,] AddRoom(int x, int y, Rooms[,] Override, int Clearance){
    int[] RandomPlace = {-1,1};
for(int i = 0; i >= 2; i++){

        if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y, Visited) == true){
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
}
        if(Clearance == i && Override[x-1, y] == null && RoomsExist(x-1,y,Visited) == true){
            if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y,Visited) == true)
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
        }
        if(Clearance == 0 && Override[x, y+1] == null && RoomsExist(x,y+1,Visited) == true){
            if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y,Visited) == true)
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
        }
        if(Clearance == 0 && Override[x, y-1] == null && RoomsExist(x,y-1,Visited) == true){
            if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y,Visited) == true)
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
        }
} 
return Override;      
}
*/
//tests if the designated room is possible inside of the array (AKA not ArrayOutOfBoundsException)

static bool RoomsExist(int x, int y, bool[,] Holder){
if(x > Holder.GetLength(0) || x < 0 || y > Holder.GetLength(1) || y < 0){
    return false;
}else{
    return true;
}
}





Console.ReadKey();

