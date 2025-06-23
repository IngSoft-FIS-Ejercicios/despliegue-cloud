import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCosmeticComponent } from './create-cosmetic.component';

describe('CreateCosmeticComponent', () => {
  let component: CreateCosmeticComponent;
  let fixture: ComponentFixture<CreateCosmeticComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateCosmeticComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateCosmeticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
