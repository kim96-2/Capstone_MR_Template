using System.Collections.Generic;
using RestAPI.GoogleObject;

namespace RestAPI.GoogleObject
{
    [System.Serializable]
    public class PlaceRequest
    {
    
    }
    
    [System.Serializable]
    public class PlaceResponse
    {
        public List<Place> places;
    }

}