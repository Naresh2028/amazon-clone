import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { catchError, EMPTY, exhaustMap, of, Subject } from 'rxjs';
import { CategoryApiService } from 'src/app/core/services/category-api.service';
import { categoryRequest } from 'src/app/core/types/category';

@Component({
  selector: 'app-category-create',
  templateUrl: './category-create.component.html',
  styleUrls: ['./category-create.component.css'],
})

// Handling duplicate form submission
export class CategoryCreateComponent implements OnInit {
  categoryForm!: FormGroup;

  private submit$ = new Subject<categoryRequest>();

  constructor(
    private router: Router,
    private categoryService: CategoryApiService,
    private fb: FormBuilder,
    private destroy: DestroyRef,
  ) {}

  ngOnInit() {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
    });

    this.submit$
      .pipe(
        exhaustMap((req) => {
          return this.categoryService.create(req).pipe(
            catchError((err: Error) => {
              return EMPTY;
            }),
          );
        }),
        takeUntilDestroyed(this.destroy),
      )
      .subscribe({
        next: (res) => {
          alert('category created successfully');
          this.router.navigateByUrl('/');
        },
      });
  }

  CreateCategory(form: FormGroup) {
    if (!form.valid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    const request: categoryRequest = form.getRawValue();

    this.submit$.next(request);

    // this.categoryService.create(request).subscribe({
    //   next: () => {
    //     alert('Form created successfully');
    //     this.router.navigateByUrl('/');
    //   },
    //   error: (err) => {
    //     alert(`${err?.error?.message ?? 'something went wrong!'}`);
    //   },
    // });
  }
}
