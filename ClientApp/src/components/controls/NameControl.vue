<template>
  <TextControl
    :model="model"
    label="core.name"
    :description="description"
    :minLength="0"
    :maxLength="40"
    :showDescription="showDescription"
    required
  />
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import TextControl from './TextControl.vue'

@Component({
  components: {
    TextControl,
  },
})
export default class NameControl extends Vue {
  @Prop(Object)
  private readonly model: { value: string } = { value: '' }

  @Prop(String)
  private readonly description: string = ''

  @Prop(Boolean)
  private readonly showDescription?: boolean
}
</script>
