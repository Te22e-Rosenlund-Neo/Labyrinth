using System.Linq.Expressions;
using System.Runtime.CompilerServices;
List<Items> AllItems;
Rooms[,] RoomArea = new Rooms[10,10];
Random Rgen = new();
Rooms Start = new(true, true, true, true, -1);    //4 openings

Rooms Corridor = new(true, false, true, false, 0); // 2 openings
Rooms[] Clearance0 = { Start ,Corridor};

Rooms Kitchen = new(true, true, false, false, 1); //  2 openings
Rooms Bedroom = new(true, false, false, false, 1); // 1 openings
Rooms BathingHall = new(true, false, true, false, 1); // 2 openings
Rooms Library = new(true, true, true, false, 1); // 3 openings
Rooms[] Clearance1 = {Kitchen, Bedroom, BathingHall, Library};

Rooms TortureChamber = new(true, false, false, false, 2); // 1 openings
Rooms MainHall = new(true ,true, true, true, 2); // 4 openings
Rooms BathRoom = new(true, false, true, false, 2); // 2 openings
Rooms[] Clearance2 = {TortureChamber, MainHall, BathRoom};

Rooms Garden = new(true, false, false, false, 3); // 1 openings EXIT
Rooms[] Final = {Garden};

Rooms[][] Clearances = {Clearance0, Clearance1, Clearance2, Final};

RoomArea[5,5] = Start;



Rooms[,] AddRoom(int x, int y, Rooms[,] Override, int Clearance){
    int[] RandomPlace = {-1,1};
for(int i = 0; i >= 2; i++){
        if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y) == true)
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
        if(Clearance == i && Override[x-1, y] == null && RoomsExist(x-1,y) == true){
            if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y) == true)
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
        }
        if(Clearance == 0 && Override[x, y+1] == null && RoomsExist(x,y+1) == true){
            if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y) == true)
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
        }
        if(Clearance == 0 && Override[x, y-1] == null && RoomsExist(x,y-1) == true){
            if(Clearance == i && Override[x+1, y] == null && RoomsExist(x+1,y) == true)
            if(Clearance != 0){
            Override[x+1,y] = Clearances[i+RandomPlace[Random.Shared.Next(0,1)]][Random.Shared.Next(0, Clearances.Length-1)];
}else{
    Override[x-1,y] = Clearances[i+1][Random.Shared.Next(0, Clearances.Length-1)];
}
        }
} 
return Override;      
}

//tests if the designated room is possible inside of the array (AKA not ArrayOutOfBoundsException)
bool RoomsExist(int x, int y){
if(x > 9 || x < 0 || y > 9 || y < 0){
    return false;
}else{
    return true;
}
}








Console.ReadKey();

