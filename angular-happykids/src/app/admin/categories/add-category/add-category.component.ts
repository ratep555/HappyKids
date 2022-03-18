import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CategoriesService } from '../categories.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.scss']
})
export class AddCategoryComponent implements OnInit {
  categoryForm: FormGroup;

  constructor(private categoriesService: CategoriesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.categoryForm = this.fb.group({
      name: ['', [Validators.required]]
     });
  }

  onSubmit() {
    if (this.categoryForm.invalid) {
        return;
    }
    this.createCategory();
  }

  private createCategory() {
    this.categoriesService.createCategory(this.categoryForm.value).subscribe(() => {
      this.router.navigateByUrl('categories');
    },
    error => {
      console.log(error);
    });
  }

}
