

class Rooms{
public string RoomsName;
public int RoomClearanceLvl;
//up left down right





public Rooms(int RoomClearanceLvl, string name){
    
    this.RoomClearanceLvl = RoomClearanceLvl;
    RoomsName = name;
}




public static void RoomDescription(string RoomsName){

    Console.WriteLine($"You Entered {RoomsName}");
    Console.WriteLine(@"			top room



	Left room			Right Room

		
			Bottom room");

}







}