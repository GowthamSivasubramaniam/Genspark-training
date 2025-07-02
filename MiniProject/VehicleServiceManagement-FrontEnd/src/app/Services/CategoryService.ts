import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
@Injectable()
export class CategoryService {
  http = inject(HttpClient)
  addcategory(category: any) {
    console.log("cat")
    return this.http.post(`https://localhost:7176/api/v1/Category?amt=${category.price}`, JSON.stringify(category.name), {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Content-Type': 'application/json'
      })
    })
  }
  getAllCategories() {

    return this.http.get(`https://localhost:7176/api/v1/Category`, {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Content-Type': 'application/json'
      })
    })

  }
  updateCategory(price: number, id: string) {
    return this.http.put(`https://localhost:7176/api/v1/Category?id=${id}&Amount=${price}`, {}, {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Content-Type': 'application/json'
      })
    })
  }
 deleteCategory(id: string) {
  return this.http.delete(`https://localhost:7176/api/v1/Category/${id}`, {
    headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    })
  });
}

}