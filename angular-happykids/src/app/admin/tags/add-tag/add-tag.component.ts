import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TagsService } from '../tags.service';

@Component({
  selector: 'app-add-tag',
  templateUrl: './add-tag.component.html',
  styleUrls: ['./add-tag.component.scss']
})
export class AddTagComponent implements OnInit {
  tagForm: FormGroup;

  constructor(public tagsService: TagsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.tagForm = this.fb.group({
      name: ['', [Validators.required]]
     });
  }

  onSubmit() {
    if (this.tagForm.invalid) {
        return;
    }
    this.createTag();
  }

  private createTag() {
    this.tagsService.createTag(this.tagForm.value).subscribe(() => {
      this.router.navigateByUrl('tags');
    },
    error => {
      console.log(error);
    });
  }

}
