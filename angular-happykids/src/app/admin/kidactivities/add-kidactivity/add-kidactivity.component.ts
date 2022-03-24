import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { KidActivity } from 'src/app/shared/models/kidactivity';
import { KidactivitiesService } from '../kidactivities.service';

@Component({
  selector: 'app-add-kidactivity',
  templateUrl: './add-kidactivity.component.html',
  styleUrls: ['./add-kidactivity.component.scss']
})
export class AddKidactivityComponent implements OnInit {
  kidActivityForm: FormGroup;
  model: KidActivity;

  constructor(private kidactivitiesService: KidactivitiesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createKidActivityForm();
  }

  createKidActivityForm() {
    this.kidActivityForm = this.fb.group({
      name: [null, [Validators.required]],
      videoClip: [''],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      picture: ''
    });
  }

  onSubmit() {
    this.kidactivitiesService.createKidActivity(this.kidActivityForm.value).subscribe(() => {
      this.router.navigateByUrl('kidactivities');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.kidActivityForm.get('picture').setValue(image);
  }

}
