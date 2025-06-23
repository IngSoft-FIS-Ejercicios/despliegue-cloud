import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { CommonService } from '../../../services/CommonService';
import { twoDecimalValidator } from 'src/app/utils/validators';
import { PharmacyService } from 'src/app/services/pharmacy.service';
import { CosmeticService } from 'src/app/services/cosmetic.service';
import { Pharmacy } from 'src/app/interfaces/pharmacy';
import { Cosmetic } from 'src/app/interfaces/cosmetic';
import {
  cilBarcode,
  cilPencil,
  cilAlignCenter,
  cilDollar,
  cilTask,
} from '@coreui/icons';

@Component({
  selector: 'app-create-cosmetic',
  templateUrl: './create-cosmetic.component.html',
  styleUrls: ['./create-cosmetic.component.css'],
})
export class CreateCosmeticComponent implements OnInit {
  form: FormGroup;
  pharmacies: Pharmacy[] = [];

  descriptionCharacterCount = 0;
  nameCharacterCount = 0;
  icons = {
    cilBarcode,
    cilPencil,
    cilAlignCenter,
    cilDollar,
    cilTask,
  };

  constructor(
    private commonService: CommonService,
    private formBuilder: FormBuilder,
    private pharmacyService: PharmacyService,
    private cosmeticService: CosmeticService,
  ) {
    this.form = this.formBuilder.group({
      code: ['', [Validators.required, this.codeValidator]],
      name: ['', [Validators.required, Validators.maxLength(30)]],
      description: ['', [Validators.required, Validators.maxLength(70)]],
      price: [
        '',
        [Validators.required, twoDecimalValidator, this.priceValidator],
      ],
    });
  }

  ngOnInit(): void {
    this.getPharmacies();
  }

  getPharmacies(): void {
    this.pharmacyService.getPharmacys().subscribe((pharmacies) => {
      this.pharmacies = pharmacies;
    });
  }

  codeValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value && (isNaN(value) || value.toString().length !== 5)) {
      return { invalidCode: true };
    }
    return null;
  }

  priceValidator(control: AbstractControl): ValidationErrors | null {
    const value = parseFloat(control.value);
    if (isNaN(value) || value < 0) {
      return { invalidPrice: true };
    }
    return null;
  }

  updateDescriptionCharCount() {
    const description = this.form.get('description')?.value || '';
    this.descriptionCharacterCount = description.length;
  }

  updateNameCharCount() {
    const name = this.form.get('name')?.value || '';
    this.nameCharacterCount = name.length;
  }

  limitInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;

    if (value.length > 5) {
      input.value = value.slice(0, 5);
    }
  }

  createCosmetic(): void {
    if (this.form.valid) {
      const userString = localStorage.getItem('login');
      const user = userString ? JSON.parse(userString) : null;
      const selectedPharmacy = this.pharmacies.find(
        (pharmacy) => pharmacy.id.toString() === user.pharmacyId.toString(),
      );
      const price = parseFloat(this.form.value.price);
      const formattedPrice = isNaN(price) ? 0 : parseFloat(price.toFixed(2));

      const cosmetic: Cosmetic = {
        code: this.form.value.code,
        name: this.form.value.name,
        description: this.form.value.description,
        price: formattedPrice,
        pharmacyName: selectedPharmacy?.name || '',
      };

      this.cosmeticService.createCosmetic(cosmetic).subscribe((c) => {
        if (c) {
          this.commonService.updateToastData(
            `Cosmetic created successfully`,
            'success',
            'Cosmetic',
          );

          this.form.reset();
        }
      });
    }
  }
}
