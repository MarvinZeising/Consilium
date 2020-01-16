<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'core.edit'"
      />
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'shift.category.update'"
          />
        </v-card-title>
        <v-card-text>

          <p
            class="subtitle-1"
            v-t="'shift.category.updateDescription'"
          />

          <p v-t="'shift.category.nameDescription'" />
          <v-text-field
            v-model="name"
            :label="$t('core.name')"
            :rules="nameRules"
            filled
            required
          />

        </v-card-text>
        <v-card-actions>
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <v-spacer />
          <DeleteCategoryDialog :category="category" />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import CategoryModule from '../../store/categories'
import { Role, Category } from '../../models'
import DeleteCategoryDialog from './DeleteCategoryDialog.vue'

@Component({
  components: {
    DeleteCategoryDialog,
  },
})
export default class UpdateCategoryDialog extends Vue {
  private categoryModule: CategoryModule = getModule(CategoryModule, this.$store)

  @Prop(Category)
  private readonly category?: Category

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 })
  ]

  private created() {
    this.name = this.category?.name || ''
  }

  private async save() {
    if (this.valid && this.category) {
      this.loading = true

      await this.categoryModule.updateCategory({
        categoryId: this.category.id,
        name: this.name,
      })

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
