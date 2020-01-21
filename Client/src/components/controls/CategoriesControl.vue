<template>
  <v-toolbar-items>

    <v-select
      v-model="selectedCategories"
      :items="categories"
      :label="$tc('shift.category.categories', 1)"
      class="ma-3"
      multiple
      outlined
      dense
    >
      <template v-slot:prepend-item>
        <v-list-item
          ripple
          @click="toggle"
        >
          <v-list-item-action>
            <v-icon
              :color="selectedCategories.length > 0 ? 'primary' : ''"
              v-html="getIcon"
            />
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title v-t="'core.selectAll'" />
          </v-list-item-content>
        </v-list-item>
        <v-divider class="mt-2"></v-divider>
      </template>
    </v-select>

  </v-toolbar-items>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'

@Component
export default class CategoriesControl extends Vue {

  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(String)
  private readonly description?: string

  @Prop(Boolean)
  private readonly isRequired?: boolean

  private get getIcon () {
    if (this.selectedCategories.length === this.categories.length) {
      return 'check_box'
    } else if (this.selectedCategories.length > 0) {
      return 'indeterminate_check_box'
    }
    return 'check_box_outline_blank'
  }

  private rules = [
    (v: string) => this.isRequired === false
                   || !!v
                   || i18n.t('core.fieldRequired'),
  ]

  private categories = [
    'Trolley',
    'Infostand',
    'FFD',
    'Hafendienst',
  ]

  private selectedCategories: string[] = []

  private toggle() {
    if (this.selectedCategories.length === this.categories.length) {
      this.selectedCategories = []
    } else {
      this.selectedCategories = this.categories
    }
  }

}
</script>
