import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CategoryApiService } from 'src/app/core/services/category-api.service';
import { categoryRequest } from 'src/app/core/types/category';

@Component({
  selector: 'app-category-create',
  templateUrl: './category-create.component.html',
  styleUrls: ['./category-create.component.css']  
})
export class CategoryCreateComponent implements OnInit {
  categoryForm!: FormGroup;

  constructor(
    private router: Router,
    private categoryService: CategoryApiService,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
    });
  }

  CreateCategory(form: FormGroup) {
    if (!form.valid) {
      alert('Form is invalid');
      return;
    }

    const request: categoryRequest = form.getRawValue();

    this.categoryService.create(request).subscribe({
      next: () => {
        alert('Form created successfully');
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        alert(`${err?.error?.message ?? 'something went wrong!'}`);
      },
    });
  }
}
