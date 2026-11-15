import { Component, OnInit, DestroyRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryApiService } from 'src/app/core/services/category-api.service';
import { categoryRequest, categoryResponse } from 'src/app/core/types/category';
import { EMPTY, switchMap, takeUntil } from 'rxjs';
import {NgForm} from '@angular/forms';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-category-update',
  templateUrl: './category-update.component.html',
  styleUrls: ['./category-update.component.css'],
})
export class CategoryUpdateComponent implements OnInit {
  categoryId: string = '';
  categoryFormData!: categoryResponse;  

  constructor(
    private categoryService: CategoryApiService,
    private route: ActivatedRoute,
    private router:Router,
    private destroyRef:DestroyRef
  ) {}

  ngOnInit() {
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          const id = params.get('id');

          if (!id) {
            return EMPTY;
          }

          this.categoryId = id;
          return this.categoryService.getById(this.categoryId);
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: (res) => {
          this.categoryFormData = res;
        },
        error: (err) => {
          alert(err?.error?.error ?? 'something went wrong!');
        },
      });
  }

  updateCategoryForm(form:NgForm){

    if(form.invalid){
      alert('form is invalid');
      return;
    }

    const request : categoryRequest = form.value;
    
    this.categoryService.update(this.categoryId,request).subscribe({
      next:() => {
        alert('form updated successfully!');
        this.router.navigateByUrl('/');
      },
      error:(err) => {
        alert(err?.error?.error ?? err?.error?.message ?? 'something went wrong!');
      }
    })
  }

  
}
