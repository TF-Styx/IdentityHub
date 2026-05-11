import { CommonModule } from "@angular/common";
import { Component, inject, OnInit, signal } from "@angular/core";
import { ProfileGeneralInfoComponent } from "../../features/profile/general-info/profile-general-info.component";
import { FileUploadComponent } from "../../features/file/file-upload.component";
import { firstValueFrom } from "rxjs";
import { ProfileApi } from "./profile.api";
import { ProfileGeneralInfoResponse } from "../../entities/user/model/types";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
  standalone: true,
  imports: [CommonModule, ProfileGeneralInfoComponent, FileUploadComponent],
})

export class ProfilePage implements OnInit {
    private profileApi = inject(ProfileApi);

    isLoaded = signal(false);
    isSubmitting = signal(false);
    userData = signal<Partial<ProfileGeneralInfoResponse>>({});
    avatarUrl = signal<string | null>(null);
    message = signal<string | null>(null);
  
    private pendingAvatarFile: File | null = null;

    ngOnInit(): void {
        this.loadData();
    }

    private loadData(): void {
        this.profileApi.getProfileGeneralInfo().subscribe({
        next: (profile) => {
            this.userData.set(profile);
            // this.avatarUrl.set(profile.avatarUrl || null);
            this.isLoaded.set(true);
        },
        error: () => this.message.set('Ошибка загрузки профиля')
        });
    }

    onAvatarSelected(file: File): void {
        this.pendingAvatarFile = file;
    }

    async onSaveGeneralInfo(payload: { userName: string }): Promise<void> {
        this.isSubmitting.set(true);
        this.message.set(null);

        try {
        this.profileApi.updateUserName({userName: payload.userName})
            .subscribe({
                next: () => {
                    alert('Изменение имени пользователя прошло успешно!')
                    // this.userNameDB = newUserName
                },
                error: errors => console.log(errors)
            });
        
        // 2. Загружаем аватар, если выбран
        if (this.pendingAvatarFile) {
            const fromData = new FormData();
            fromData.append('File', this.pendingAvatarFile, this.pendingAvatarFile.name);

            await firstValueFrom(this.profileApi.uploadAvatar(fromData));
            
            this.avatarUrl.set(URL.createObjectURL(this.pendingAvatarFile));
            this.pendingAvatarFile = null;
        }

        this.message.set('Данные успешно сохранены');
        this.loadData(); // перечитываем актуальные данные
        } catch (err) {
        this.message.set('Ошибка при сохранении');
        } finally {
        this.isSubmitting.set(false);
        }
    }

    onCancel(): void {
        this.message.set(null);
        // можно сбросить форму или закрыть страницу
    }
}
