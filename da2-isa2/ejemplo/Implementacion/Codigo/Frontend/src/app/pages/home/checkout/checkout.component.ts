import { Component, OnInit } from '@angular/core';
import { cilThumbUp, cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { Router } from '@angular/router';
import { PurchaseService } from '../../../services/purchase.service';
import { StorageManager } from '../../../utils/storage-manager';
import {
  PurchaseRequest,
  PurchaseRequestDetail,
} from 'src/app/interfaces/purchase';
import { CommonService } from '../../../services/CommonService';
import { Drug } from 'src/app/interfaces/drug';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
})
export class CheckoutComponent implements OnInit {
  total: number = 0;
  email: string = '';
  cart: Drug[] = [];

  constructor(
    public iconSet: IconSetService,
    private router: Router,
    private purchaseService: PurchaseService,
    private storageManager: StorageManager,
    private commonService: CommonService,
  ) {
    iconSet.icons = { cilThumbUp, cilCart, cilPlus, cilCompass };
  }

  ngOnInit(): void {
    this.updateCart();
    const _total = JSON.parse(this.storageManager.getData('total'));
    this.total = 0;
    if (_total) {
      this.total = _total;
    }
  }

  finishPurchase(): void {
    const cart = JSON.parse(this.storageManager.getData('cart'));
    const details: PurchaseRequestDetail[] = [];
    for (const item of cart) {
      const detail = new PurchaseRequestDetail(
        item.code,
        item.quantity,
        item.pharmacy.id,
      );
      details.push(detail);
    }

    const now = new Date().toISOString();
    const purchaseRequest = new PurchaseRequest(this.email, now, details);
    this.purchaseService.addPurchase(purchaseRequest).subscribe((purchase) => {
      if (purchase) {
        console.log(purchase);
        this.commonService.updateToastData(
          'Tracking code: ' + purchase.trackingCode,
          'success',
          'Thank you for your purchase.',
        );
        this.storageManager.removeData('cart');
        this.router.navigate(['/home']);
      }
    });
  }

  updateCart(): void {
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    if (!this.cart) {
      this.cart = [];
      this.storageManager.saveData('cart', JSON.stringify(this.cart));
    }
  }
}
