import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BirthdaypackagesService } from 'src/app/admin/birthdaypackages/birthdaypackages.service';
import { BirthdayPackage } from 'src/app/shared/models/birthdaypackage';

@Component({
  selector: 'app-birthdaypackage-detail',
  templateUrl: './birthdaypackage-detail.component.html',
  styleUrls: ['./birthdaypackage-detail.component.scss']
})
export class BirthdaypackageDetailComponent implements OnInit {
  birthdayPackage: BirthdayPackage;

  constructor(private birthdaypackagesService: BirthdaypackagesService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadBirthdayPackage();
  }

  loadBirthdayPackage() {
    return this.birthdaypackagesService.getBirthdayPackageById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.birthdayPackage = response;
    }, error => {
    console.log(error);
    });
    }

}
