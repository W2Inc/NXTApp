<script lang="ts">
	import Card from "$lib/components/cards/card.svelte";
	import Button from "$lib/components/buttons/button.svelte";
	import Modal from "$lib/components/dialogs/modal.svelte";
	import Textarea from "$lib/components/input/textarea.svelte";
	import Input from "$lib/components/input/input.svelte";
	import Select from "$lib/components/input/select.svelte";

	let salesModal: Modal;
</script>

<Modal bind:this={salesModal} title="Contact us" action="/contact">
	<div style="display: flex; flex-direction: column; gap: 12px;">
		<Input
			id="email-:sales:"
			label="Email"
			type="email"
			required
			placeholder="example@email.com"
		/>

		<div class="center">
			<Input
				id="name-:sales:"
				label="Your Name"
				type="text"
				required
				placeholder="Johnny Silverhand"
			/>
			<Input
				id="website-:sales:"
				label="Website"
				type="url"
				required
				placeholder="www.example.com"
			/>
		</div>

		<div>
			<label for="students-:sales:">Number of Students *</label>
			<Select id="students-:sales:" required>
				<option value="1-100">1-100</option>
				<option value="100-500">100-500</option>
				<option value="500-1000">500-1000</option>
				<option value="1000+">1000+</option>
			</Select>
		</div>
	</div>

	<label for="students-:sales:">How can we help?</label>
	<Textarea
		vertical
		style="max-height: 256px;"
		maxlength={512}
		autocapitalize="off"
		autocomplete="off"
		autocorrect="off"
		spellcheck="false"
		name="note"
		rows={8}
	/>

	<Button style="width: 100%; margin-top: 8px;" type="submit" formmethod="post">
		Submit
	</Button>
</Modal>

<div class="container">
	<section class="intro">
		<h1>Start your education today.</h1>
	</section>

	<div class="products">
		<Card>
			<h1>Free</h1>
			<p>The basics for individuals and tryouts</p>
			<p class="price"><span>0€</span></p>
			<Button href="">Coming Soon</Button>
			<hr />
			<p>Features</p>
			<ul>
				<li>Join 1 learning path</li>
				<li>Free access to our community</li>
				<li>Free for the first 3 learning goals</li>
			</ul>
		</Card>

		<Card hightlight>
			<h1>Student</h1>
			<p>Advanced collaboration for individuals</p>
			<p class="price"><span>25€</span> per user/year</p>
			<Button href="">Coming Soon</Button>
			<hr />
			<p>Features</p>
			<ul>
				<li>Unlimited learning paths</li>
				<li>Create your own path</li>
				<li>Create your own goals</li>
				<li>Create your own projects</li>
				<li>Create your own rubrics</li>
				<li>Free access to our community</li>
			</ul>
		</Card>

		<Card>
			<h1>Enterprise</h1>
			<p>Enroll an entire campus with us!</p>
			<p class="price"><span>20€</span> per user/month</p>
			<Button type="submit" on:click={() => salesModal.show()}>Contact Sales</Button>
			<hr />
			<p>Features</p>
			<ul>
				<li>Constant support</li>
				<li>Monitoring over students</li>
				<li>Custom feature requests</li>
				<li>Software support</li>
			</ul>
		</Card>
	</div>

	<hr />

	<Card></Card>
</div>

<style>
	ul {
		list-style-type: "✅ ";
	}

	section.intro {
		text-align: center;
		padding: var(--padding);

		& h1 {
			font-size: 64px;
			font-weight: 400;
			color: var(--foreground-color);
			text-align: center;
		}
	}

	div.container {
		margin: 0 auto;
		padding: var(--padding);
		max-width: calc(var(--max-content-width) * 1.5);
	}

	div.products {
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
		flex-wrap: wrap;
		justify-content: center;
		gap: 16px;

		@media (max-width: 768px) {
			grid-auto-flow: row;
		}

		& h1 {
			font-size: 2rem;
			font-weight: 400;
			color: var(--foreground-color);
			text-align: center;
		}

		& p {
			max-width: 576px;
			font-weight: 400;
			color: var(--text-2);

			&.price {
				padding-top: 8px;
				font-weight: 400;
				color: var(--text-3);

				& span {
					font-size: 2rem;
					font-weight: 400;
					color: var(--foreground-color);
				}
			}
		}
	}

	@media (max-width: 768px) {
		div.products {
			flex-direction: column;
		}
	}
</style>
