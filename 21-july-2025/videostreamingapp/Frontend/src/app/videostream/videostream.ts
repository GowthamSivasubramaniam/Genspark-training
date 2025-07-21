import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { VideoService } from '../Services/Videoservice';
import { CommonModule } from '@angular/common';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-videostream',
  imports: [ReactiveFormsModule,CommonModule,FormsModule],
  templateUrl: './videostream.html',
  styleUrl: './videostream.css'
})
export class Videostream {
  videoaddform:FormGroup
  videos:any[] =[]
  showform:boolean=false
  query:string=""
  constructor( private ser:VideoService)
  {
    this.videoaddform = new FormGroup({
      Name:new FormControl("", [Validators.required]),
      Description: new FormControl(null, [Validators.required]),
      Video: new FormControl(null, [Validators.required])
    });
     this.ser.getVideos("").subscribe(
      {
          next:(data:any)=>
          {
            console.log(data)
            this.videos=data.$values

          }
      }
     )

  }
   public get Name() {
    return this.videoaddform.get('Name');
  }
  
   public get Description() {
    return this.videoaddform.get('Description');
  }

  public get Video() {
    return this.videoaddform.get('Video');
  }

    onVideoSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.Video?.setValue(file);
      this.Video?.markAsTouched();
    }
  }
  isshowform()
  {
    this.showform=true
  }
  ishideform()
  {
    this.showform=false
  }
loading=false
  upload()
  {
    console.log("uploading")
     const formData = new FormData();
    formData.append('Name', this.Name?.value);
    formData.append('Description', this.Description?.value);

    const file = this.videoaddform.get('Video')?.value;
    if (file) {
      formData.append('Video', file, file.name);
    }
    for (const [key, value] of formData.entries()) {
  console.log(key, value);
}
   this.loading=true
    this.ser.addVideo(formData).subscribe(
      {
        next:(data:any)=>
        {
          alert("data added")
          this.loading=false
        },
        error:(err:any)=>
        {
          console.error(err)
        }

      }
    )
  }
  selectedVideo: any = null;

openVideoModal(video: any) {
  this.selectedVideo = video;
}

closeVideoModal() {
  this.selectedVideo = null;
}
search()
{
  this.ser.getVideos(this.query).pipe(
    debounceTime(2000)
  ).subscribe(
      {
          next:(data:any)=>
          {
            console.log(data)
            this.videos=data.$values

          }
      }
     )
}
}
