import { Component, Input, OnInit } from '@angular/core';
import { BirthdayPackage } from 'src/app/shared/models/birthdaypackage';

@Component({
  selector: 'app-birthdaypackage',
  templateUrl: './birthdaypackage.component.html',
  styleUrls: ['./birthdaypackage.component.scss']
})
export class BirthdaypackageComponent implements OnInit {
  @Input() birthdayPackage: BirthdayPackage;

  constructor() { }

  ngOnInit(): void {
  }

}
