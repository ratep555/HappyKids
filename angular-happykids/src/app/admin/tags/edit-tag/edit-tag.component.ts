import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Tag } from 'src/app/shared/models/tag';
import { TagsService } from '../tags.service';

@Component({
  selector: 'app-edit-tag',
  templateUrl: './edit-tag.component.html',
  styleUrls: ['./edit-tag.component.scss']
})
export class EditTagComponent implements OnInit {
  tagForm: FormGroup;
  tag: Tag;
  id: number;

  constructor(private tagsService: TagsService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.tagForm = this.fb.group({
      id: [this.id],
      name: ['', [Validators.required]]
     });

    this.tagsService.getTagById
    (this.id)
    .pipe(first())
    .subscribe(x => this.tagForm.patchValue(x));
  }

  onSubmit() {
    if (this.tagForm.invalid) {
        return;
    }
    this.updateTag();
  }

  private updateTag() {
    this.tagsService.updateTag
    (this.id, this.tagForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('tags');
          }, error => {
            console.log(error);
          });
        }

}
