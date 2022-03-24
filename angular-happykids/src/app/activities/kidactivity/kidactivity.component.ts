import { Component, Input, OnInit } from '@angular/core';
import { KidActivity } from 'src/app/shared/models/kidactivity';

@Component({
  selector: 'app-kidactivity',
  templateUrl: './kidactivity.component.html',
  styleUrls: ['./kidactivity.component.scss']
})
export class KidactivityComponent implements OnInit {
  @Input() kidActivity: KidActivity;

  constructor() { }

  ngOnInit(): void {
  }

}
