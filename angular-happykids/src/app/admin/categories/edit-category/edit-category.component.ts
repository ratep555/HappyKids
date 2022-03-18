import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Category } from 'src/app/shared/models/category';
import { CategoriesService } from '../categories.service';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.scss']
})
export class EditCategoryComponent implements OnInit {
  categoryForm: FormGroup;
  category: Category;
  id: number;

  constructor(private categoriesService: CategoriesService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.categoryForm = this.fb.group({
      id: [this.id],
      name: ['', [Validators.required]]
     });

    this.categoriesService.getCategoryById
    (this.id)
    .pipe(first())
    .subscribe(x => this.categoryForm.patchValue(x));
  }

  onSubmit() {
    if (this.categoryForm.invalid) {
        return;
    }
    this.updateCategory();
  }

  private updateCategory() {
    this.categoriesService.updateCategory
    (this.id, this.categoryForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('categories');
          }, error => {
            console.log(error);
          });
        }

}
