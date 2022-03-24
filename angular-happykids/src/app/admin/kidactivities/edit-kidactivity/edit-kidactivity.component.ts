import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { KidActivity } from 'src/app/shared/models/kidactivity';
import { KidactivitiesService } from '../kidactivities.service';

@Component({
  selector: 'app-edit-kidactivity',
  templateUrl: './edit-kidactivity.component.html',
  styleUrls: ['./edit-kidactivity.component.scss']
})
export class EditKidactivityComponent implements OnInit {
  kidActivityForm: FormGroup;
  id: number;
  kidActivity: KidActivity;

  constructor(private kidactivitiesService: KidactivitiesService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.loadKidActivity();

    this.kidActivityForm = this.fb.group({
      id: [this.id],
      name: [null, [Validators.required]],
      videoClip: [''],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      picture: ''
    });

    this.kidactivitiesService.getKidActivityById(this.id)
    .pipe(first())
    .subscribe(x => this.kidActivityForm.patchValue(x));
  }

   loadKidActivity() {
    return this.kidactivitiesService.getKidActivityById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.kidActivity = response;
    }, error => {
    console.log(error);
    });
    }

  onSubmit() {
    this.kidactivitiesService.updateKidActivity(this.id, this.kidActivityForm.value).subscribe(() => {
    this.router.navigateByUrl('kidactivities');
        }, error => {
          console.log(error);
        });
      }

      onImageSelected(image){
        this.kidActivityForm.get('picture').setValue(image);
      }

}
