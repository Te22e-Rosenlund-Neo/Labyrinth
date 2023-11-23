using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

class Rooms{

 string RoomsName;
bool escapable;
int RoomClearanceLvl;
//up left down right
public bool[] DoorPossible = {false, false, false, false};


bool Collapsed = false;


public Rooms(bool Door1, bool Door2, bool Door3, bool Door4, int RoomClearanceLvl){

    bool[] TempDoorPos = {Door1, Door2, Door3, Door4};
    DoorPossible = TempDoorPos;
    this.RoomClearanceLvl = RoomClearanceLvl;
}












public void RoomDescription(){

    Console.WriteLine($"You Entered {RoomsName}");
    Console.WriteLine(@"			Top Rooms



	Left room			Right Room

		
			Bottom room");

}







}