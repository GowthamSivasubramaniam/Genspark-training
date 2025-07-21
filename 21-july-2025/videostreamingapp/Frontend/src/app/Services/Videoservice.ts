import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
@Injectable()
export class VideoService
{
   http =inject(HttpClient)

   addVideo(data:any)
   {
    return this.http.post('http://localhost:5126/api/Video',data)
}
getVideos(query:any|null)
{     
    if(query)
    {
        return this.http.get(`http://localhost:5126/api/Video?query=${query}`)
    }
       return this.http.get(`http://localhost:5126/api/Video`)
     
}
}