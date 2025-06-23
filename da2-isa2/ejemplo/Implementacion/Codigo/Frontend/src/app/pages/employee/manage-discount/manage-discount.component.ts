import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonService } from '../../../services/CommonService';
import { cilDollar, cilCart, cilTags } from '@coreui/icons';
import { ProductService } from '../../../services/product.service';
import { DiscountService } from '../../../services/discount.service'; // Importa el servicio de descuentos
import { Product } from '../../../interfaces/product';

@Component({
  selector: 'app-manage-discount',
  templateUrl: './manage-discount.component.html',
  styleUrls: ['./manage-discount.component.css'],
})
export class ManageDiscountComponent implements OnInit {
  form: FormGroup;
  icons = {
    cilDollar,
    cilCart,
    cilTags,
  };
  initialPrice = 0;
  finalPrice = 0;
  products: Product[] = [];
  selectedProduct?: Product;

  constructor(
    private commonService: CommonService,
    private formBuilder: FormBuilder,
    private productService: ProductService,
    private discountService: DiscountService, // Inyecta el servicio de descuentos
  ) {
    this.form = this.formBuilder.group({
      discount: [
        '',
        [Validators.required, Validators.min(0), Validators.max(100)],
      ],
      product: ['', Validators.required],
      discountType: [''],
    });
  }

  ngOnInit(): void {
    this.loadProducts();

    this.form.get('product')?.valueChanges.subscribe((productId) => {
      this.selectedProduct = this.products.find(
        (product) => product.id === +productId,
      );
      if (this.selectedProduct) {
        this.initialPrice = this.selectedProduct.price;
        this.finalPrice = this.initialPrice;
        this.form
          .get('discount')
          ?.setValue(this.selectedProduct.discountPercentage || null);
        this.calculateFinalPrice(this.form.get('discount')?.value);
      }
    });

    this.form.get('discount')?.valueChanges.subscribe((discount) => {
      this.calculateFinalPrice(discount);
    });

    this.form.get('discountType')?.valueChanges.subscribe(() => {
      this.calculateFinalPrice(this.form.get('discount')?.value);
    });
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe((data) => {
      this.products = data;
      console.log(this.products);
    });
  }

  applyDiscount(): void {
    if (this.form.valid && this.selectedProduct) {
      this.discountService
        .createDiscount(this.selectedProduct)
        .subscribe(() => {
          this.commonService.updateToastData(
            `Discount applied successfully`,
            'success',
            'Discount:',
          );
          this.loadProducts();
        });
    }
  }

  disableDiscount(): void {
    if (this.selectedProduct) {
      this.discountService
        .removeDiscount(this.selectedProduct)
        .subscribe(() => {
          this.commonService.updateToastData(
            `Discount disabled successfully`,
            'success',
            'Discount:',
          );
        });
    }
  }

  calculateFinalPrice(discount: number): void {
    const discountType = this.form.get('discountType')?.value;

    if (this.selectedProduct) {
      if (discountType === 'percentage') {
        this.finalPrice = this.initialPrice * (1 - discount / 100);
      } else if (discountType === 'fixed') {
        this.finalPrice = this.initialPrice - discount;
      }

      if (this.finalPrice < 0) {
        this.finalPrice = 0;
      }
    }
  }

  isApplyDiscountDisabled(): boolean {
    return this.selectedProduct?.hasDiscount === true || this.form.invalid;
  }

  isDisableDiscountDisabled(): boolean {
    return !this.selectedProduct?.hasDiscount || this.form.invalid;
  }
}
