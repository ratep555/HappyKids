import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { KidactivitiesService } from 'src/app/admin/kidactivities/kidactivities.service';
import { KidActivity } from 'src/app/shared/models/kidactivity';
import { ActivitiesService } from '../activities.service';

@Component({
  selector: 'app-kidactivity-detail',
  templateUrl: './kidactivity-detail.component.html',
  styleUrls: ['./kidactivity-detail.component.scss']
})
export class KidactivityDetailComponent implements OnInit {
  kidActivity: KidActivity;
  videoClipURL: SafeResourceUrl;

  constructor(private kidactivitiesService: KidactivitiesService,
              private activatedRoute: ActivatedRoute,
              private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.loadKidActivity();
  }

  loadKidActivity() {
    return this.kidactivitiesService.getKidActivityById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.kidActivity = response;
    this.videoClipURL = this.generateYoutubeURLForEmbeddedVideo(this.kidActivity.videoClip);
    }, error => {
    console.log(error);
    });
    }

    generateYoutubeURLForEmbeddedVideo(url: any): SafeResourceUrl{
      if (!url){
        return '';
      }
      let videoId = url.split('v=')[1];
      const ampersandPosition = videoId.indexOf('&');
      if (ampersandPosition !== -1){
        videoId = videoId.substring(0, ampersandPosition);
      }
      return this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${videoId}`);
    }


}
