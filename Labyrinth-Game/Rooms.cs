using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

class Rooms{

 string RoomsName;
bool escapable;
public int RoomClearanceLvl;
//up left down right



bool Collapsed = false;


public Rooms(int RoomClearanceLvl){
    
    this.RoomClearanceLvl = RoomClearanceLvl;
}



public void RoomDescription(){

    Console.WriteLine($"You Entered {RoomsName}");
    Console.WriteLine(@"			top room



	Left room			Right Room

		
			Bottom room");

}







}