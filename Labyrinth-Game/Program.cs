using System.Linq.Expressions;
using System.Runtime.CompilerServices;

List<Items> AllItems;
Rooms[,] RoomArea = new Rooms[10,10];

Rooms Start = new(true, true, true, true, 0);    //4 openings
Rooms Corridor = new(true, false, true, false, 0); // 2 openings
Rooms[] Clearance0 = {Start, Corridor};

Rooms Kitchen = new(true, true, false, false, 1); //  2 openings
Rooms Bedroom = new(true, false, false, false, 1); // 1 openings
Rooms BathingHall = new(true, false, true, false, 1); // 2 openings
Rooms Library = new(true, true, true, false, 1); // 3 openings
Rooms[] Clearance1 = {Kitchen, Bedroom, BathingHall, Library};

Rooms TortureChamber = new(true, false, false, false, 1); // 1 openings
Rooms MainHall = new(true ,true, true, true, 1); // 4 openings
Rooms BathRoom = new(true, false, true, false, 1); // 2 openings
Rooms[] Clearance2 = {TortureChamber, MainHall, BathRoom};

Rooms Garden = new(true, false, false, false, 1); // 1 openings EXIT



RoomArea[5,5] = Start;


bool RoomsExist(int x, int y){
if(x == 0 && x>10 && y == 0 && y>10){
    return true;
}
return false;
}





Console.ReadKey();

